using BookApp.DataAccess.Repository.iRepository;
using BookApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Areas.Admin.Controllers
{
    public class CoverTypeController : Controller
    {
        // dependency injection to create an instance of the db
        private readonly IUnitOfWork _unitOfWork;

        // Implement connection strings and tables to be retrieved
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            // retrieve all cover type details from db
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        #region Create Cover Type Section

        // Get
        [Area("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Post
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create (CoverType obj)
        {
            // validates the form if they are valid
            if (ModelState.IsValid)
            {
                // accepts the values of the table
                _unitOfWork.CoverType.Add(obj);

                // updates db
                _unitOfWork.Save();

                // using tempdata to display success message
                TempData["Success"] = "Cover type created successfully";

                // redirects back to index
                return RedirectToAction("Index");
            }
            // loads the page again if data is invalid
            return View(obj);
        }

        #endregion

        #region Edit Cover Type Section
        [Area("Admin")]
        public IActionResult Edit (int id)
        {
            // GET
            // checks if id is valid
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            // checks if the id exists in the db
            var CoverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);

            if (CoverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(CoverTypeFromDb);
        }

        // HTTP
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (CoverType obj)
        {
            // validates the form if they are valid
            if (ModelState.IsValid)
            {
                // accepts the values of the table
                _unitOfWork.CoverType.Update(obj);

                // updates db
                _unitOfWork.Save();

                // using tempdata to display success message
                TempData["Success"] = "Cover type Edited successfully";

                // redirects back to index
                return RedirectToAction("Index");
            }
            // loads the page again if data is invalid
            return View(obj);
        }

        #endregion

        #region Delete Cover Type Section
        [Area("Admin")]
        public IActionResult Delete (int id)
        {
            // GET
            // checks if id is valid
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            // checks if the id exists in the db
            var CoverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);

            if (CoverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(CoverTypeFromDb);
        }

        // HTTP
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Area("Admin")]
        public IActionResult DeletePost(int id)
        {
            // checks if the id exists in the db
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);

            // validates the form if they are valid
            if (obj == null)
            {
                return NotFound();
            }
                // accepts the values of the table
                _unitOfWork.CoverType.Remove(obj);

                // updates db
                _unitOfWork.Save();

                // using tempdata to display success message
                TempData["Success"] = "Cover type Deleted successfully";

                // redirects back to index
                return RedirectToAction("Index");
            

        }

        #endregion

    }
}
