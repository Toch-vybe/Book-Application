using BookApp.DataAccess.Repository.iRepository;
using BookApp.Models;
using BookApp.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookApp.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // dependency injection to create an instance of the db
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        // Implement connection strings and tables to be retrieved
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }


        #region Upsert Section
        // GET
        [Area("Admin")]
        public IActionResult Upsert(int? id) //upsert is a mixture of update and create
        {
            ProductVM productVM = new()
            {
                Product = new(),
                // for displaying dropdown in category field in the form
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                // for displaying dropdown in cover type field in the form
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };


            // checks if id already exists
            if (id == 0 || id == null)
            {
                // create product if Id does not exist
                //ViewBag.CategoryList = CategoryList;
                // ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //update product if Id exists
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }
            
        }

        // HTTP
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Area("Admin")]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            // validates the form if they are valid
            if (ModelState.IsValid)
            {
                // accepts the image from the form
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null) // checks if file is empty
                {
                    string fileName = Guid.NewGuid().ToString(); // generates a random name for the file, to prevent conflict with product with same name
                    var uploads = Path.Combine(wwwRootPath, @"images\products"); // to store the file in wwwroots folder
                    var extension = Path.GetExtension(file.FileName); // gets extenstion

                    if(obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create)) //copy uploaded file into product folder
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension; // to be added to db
                }
                if (obj.Product.Id == 0)
                {
                    // adds form details to db
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    // update form details to db
                    _unitOfWork.Product.Update(obj.Product);
                }

                // updates db
                _unitOfWork.Save();

                // using tempdata to display success message
                TempData["Success"] = "Product uploaded successfully";

                // redirects back to index
                return RedirectToAction("Index");
            }
            // loads the page again if data is invalid
            return View(obj);
        }

        #endregion

        #region API CALLS   
        // retrieve all cover type details from db
        [HttpGet]
        [Area("Admin")]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new {data = productList});
        }
        // HTTP
        [HttpDelete]
        [Area("Admin")]
        public IActionResult Delete(int? id)
        {
            // checks if the id exists in the db
            var obj = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            // validates the form if they are valid
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            // delete the image from the folder
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            // remove the values from the table in the db
            _unitOfWork.Product.Remove(obj);

            // updates db
            _unitOfWork.Save();

            // display success message
            return Json(new { success = true, message = "Delete successful" });

            // redirects back to index
            //return RedirectToAction("Index");
        }
        #endregion
    }
}
