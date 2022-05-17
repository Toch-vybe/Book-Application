using BookApp.DataAccess;
using BookApp.DataAccess.Repository.iRepository;
using BookApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Controllers
{
    public class CategoryController : Controller
    {
        // dependency injection to create an instance of the db
        private readonly IUnitOfWork _unitOfWork;

        // implement connection strings and tables to be retrieved.
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            // retrieve all category details from db
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        #region Create category section
        //Get
        [Area("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Area("Admin")]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display order cannot be the same with Name");
            }
            // validates the form if they are valid
            if (ModelState.IsValid)
            {
                // accepts the values of the table
                _unitOfWork.Category.Add(obj);

                // updates db
                _unitOfWork.Save();

                // using tempdata to display success message
                TempData["success"] = "Category created successfully";

                // redirects back to index
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        #endregion

        #region Edit category section

        //Get
        [Area("Admin")]
        public IActionResult Edit(int? id)
        {
            // checks if id is valid
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // checks if id exist in db
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id==id);


            // checks the result of the query 
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            // sends the result to the view
            return View(categoryFromDb); 
        }

        //Post
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display order cannot be the same with Name");
            }
            // validates the form if they are valid
            if (ModelState.IsValid)
            {
                // accepts the values of the table
                _unitOfWork.Category.Update(obj);

                // updates db
                _unitOfWork.Save();

                // using tempdata to display success message
                TempData["success"] = "Category Updated successfully";

                // redirects back to index
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        #endregion

        #region Delete category section
        //Get
        [Area("Admin")]
        public IActionResult Delete(int? id)
        {
            // checks if id is valid
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // checks if id exist in db
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id==id);

            // checks the result of the query 
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            // sends the result to the view
            return View(categoryFromDb);
        }

        //Post
        [Area("Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            // checks result of query
            if (obj == null)
            {
                return NotFound();
            }

            // receives confirmation for delete
            _unitOfWork.Category.Remove(obj);

            // updates db
            _unitOfWork.Save();

            // using tempdata to display success message
            TempData["success"] = "Category created successfully";

            // redirects back to index
            return RedirectToAction("Index");
        }
        #endregion
    }
}
