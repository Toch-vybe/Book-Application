using BookApp.DataAccess.Repository.iRepository;
using BookApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Areas.Admin.Controllers
{
    public class CompanyController : Controller
    {
        // dependency injection to create an instance of the db
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Area("Admin")]
        #region Upsert

        public IActionResult Upsert(int? id)
        {
            Company company = new();
            // checks if id already exists
            if (id == 0 || id == null)
            {
                // create product if Id does not exist
                return View(company);
            }
            else
            {
                //update product if Id exists
                company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
            }
        }

        // HTTP
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Area("Admin")]
        public IActionResult Upsert(Company obj, IFormFile? file)
        {
            // validates the form if they are valid
            if (ModelState.IsValid)
            {

                if (obj.Id == 0)
                {
                    // adds form details to db
                    _unitOfWork.Company.Add(obj);
                    TempData["Success"] = "Company created successfully";
                }
                else
                {
                    // update form details to db
                    _unitOfWork.Company.Update(obj);
                    TempData["Success"] = "Company created successfully";
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

        #region Api calls

        // retrieve all cover type details from db
        [HttpGet]
        [Area("Admin")]
        public IActionResult GetAll()
        {
            var CompanyList = _unitOfWork.Company.GetAll();
            return Json(new { data = CompanyList });
        }

        // HTTP
        [HttpDelete]
        [Area("Admin")]
        public IActionResult Delete(int? id)
        {
            // checks if the id exists in the db
            var obj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);

            // validates the form if they are valid
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            // remove the values from the table in the db
            _unitOfWork.Company.Remove(obj);

            // updates db
            _unitOfWork.Save();

            // display success message
            return Json(new { success = true, message = "Delete successful" });

            // redirects back to index
            //return RedirectToAction("Index");

            #endregion
        }
    }
}
