using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PostcardsManager.Models;
using PostcardsManager.Repositories;
using System;
using PostcardsManager.ViewModels;

namespace PostcardsManager.Controllers
{
    public class PhotographersController : Controller
    {   
        // GET: Photographer
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FNameSortParm = string.IsNullOrEmpty(sortOrder) ? "fname_desc" : "";
            ViewBag.LNameSortParm = string.IsNullOrEmpty(sortOrder) ? "lname_desc" : "";

            if (searchString == null)
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var photographersRepository = new PhotographerRepository();
            IDisposable context;

            var photographers = photographersRepository.GetAll(out context);

            using (context)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    photographers = photographers.Where(s => s.LastName.Contains(searchString)
                                                             || s.FirstName.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "lname_desc":
                        photographers = photographers.OrderByDescending(s => s.LastName);
                        break;
                    case "fname_desc":
                        photographers = photographers.OrderByDescending(s => s.FirstName);
                        break;
                    default: // Name ascending 
                        photographers = photographers.OrderBy(s => s.LastName);
                        break;
                }

                return View(photographers.ToList().Select(x => new PhotographerViewModel(x)));
            }
        }

        // GET: Photographer/Details/5
        public ActionResult Details(int id)
        {
            var photographersRepository = new PhotographerRepository();

            var photographer = photographersRepository.GetById(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(new PhotographerViewModel(photographer));
        }

        // GET: Photographer/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photographer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "LastName, FirstName")] PhotographerViewModel photographerVm)
        {
            var photographer = new Photographer()
            {
                Id = photographerVm.Id,
                FirstName = photographerVm.FirstName,
                MiddleName = photographerVm.MiddleName,
                LastName = photographerVm.LastName
            };

            try
            {
                if (ModelState.IsValid)
                {
                    var photographersRepository = new PhotographerRepository();
                    photographersRepository.Add(photographer);

                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("",
                    "[[[Unable to save changes. Try again, and if the problem persists, see your system administrator.]]]");
            }
            return View(photographer);
        }

        // GET: Photographer/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var photographersRepository = new PhotographerRepository();

            var photographer = photographersRepository.GetById(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(photographer);
        }

        // POST: Photographer/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditPost(int id)
        {
            var photographersRepository = new PhotographerRepository();
            var photographerToUpdate = photographersRepository.GetById(id);

            if (TryUpdateModel(photographerToUpdate, "",
                new[] {"LastName", "FirstName"}))
            {
                try
                {
                    photographersRepository.Update(photographerToUpdate);

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(photographerToUpdate);
        }

        // GET: Photographer/Delete/5
        [Authorize]
        public ActionResult Delete(int id, bool? saveChangesError = false)
        {
            var photographersRepository = new PhotographerRepository();
            var photographer = photographersRepository.GetById(id);

            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage =
                    "[[[Delete failed. Try again, and if the problem persists see your system administrator.]]]";
            }
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(photographer);
        }

        // POST: Photographer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                var photographersRepository = new PhotographerRepository();
                
                photographersRepository.Delete(id);
            }
            catch (RetryLimitExceededException /* dex */)
            {
                return RedirectToAction("Delete", new {id, saveChangesError = true});
            }
            return RedirectToAction("Index");
        }
    }
}