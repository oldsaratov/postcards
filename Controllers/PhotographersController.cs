using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PostcardsManager.DAL;
using PostcardsManager.Models;

namespace PostcardsManager.Controllers
{
    public class PhotographersController : Controller
    {
        private readonly PostcardContext db = new PostcardContext();
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

            var photographers = from s in db.Photographers
                select s;
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

            return View(photographers.ToList());
        }

        // GET: Photographer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var photographer = db.Photographers.Find(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(photographer);
        }

        // GET: Photographer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photographer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName, FirstName")] Photographer photographer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Photographers.Add(photographer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("",
                    "[[[Unable to save changes. Try again, and if the problem persists, see your system administrator.]]]");
            }
            return View(photographer);
        }

        // GET: Photographer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var photographer = db.Photographers.Find(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(photographer);
        }

        // POST: Photographer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToUpdate = db.Photographers.Find(id);
            if (TryUpdateModel(studentToUpdate, "",
                new[] {"LastName", "FirstName"}))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Photographer/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage =
                    "[[[Delete failed. Try again, and if the problem persists see your system administrator.]]]";
            }
            var photographer = db.Photographers.Find(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(photographer);
        }

        // POST: Photographer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var photographer = db.Photographers.Find(id);
                db.Photographers.Remove(photographer);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException /* dex */)
            {
                return RedirectToAction("Delete", new {id, saveChangesError = true});
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}