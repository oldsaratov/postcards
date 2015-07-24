using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PostcardsManager.DAL;
using PostcardsManager.Models;
using Resourses;

namespace PostcardsManager.Controllers
{
    public class SeriesController : Controller
    {
        private readonly PostcardContext db = new PostcardContext();
        // GET: Series
        public ActionResult Index(int? selectedPublisher)
        {
            var publishers = db.Publishers;
            ViewBag.SelectedPublisher = new SelectList(publishers, "PublisherId", "Name", selectedPublisher);
            var publisherId = selectedPublisher.GetValueOrDefault();

            var series = db.Series
                .Where(c => !selectedPublisher.HasValue || c.PublisherId == publisherId)
                .OrderBy(d => d.Id)
                .Include(d => d.Publisher);

            return View(series.ToList());
        }

        // GET: Series/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var series = db.Series.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            return View(series);
        }

        public ActionResult Create()
        {
            PopulatePublishersDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SeriesID,FrontTitle,Year,PublisherId")] Series series)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Series.Add(series);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("",
                    Resources
                        .SeriesController_Create_Unable_to_save_changes__Try_again__and_if_the_problem_persists__see_your_system_administrator_);
            }
            PopulatePublishersDropDownList(series.PublisherId);
            return View(series);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var series = db.Series.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            PopulatePublishersDropDownList(series.PublisherId);
            return View(series);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseToUpdate = db.Series.Find(id);
            if (TryUpdateModel(courseToUpdate, "",
                new[] {"FrontTitle", "Year", "PublisherId"}))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("",
                        Resources
                            .SeriesController_Create_Unable_to_save_changes__Try_again__and_if_the_problem_persists__see_your_system_administrator_);
                }
            }
            PopulatePublishersDropDownList(courseToUpdate.PublisherId);
            return View(courseToUpdate);
        }

        private void PopulatePublishersDropDownList(object selectedPublisher = null)
        {
            var publishersQuery = from d in db.Publishers
                select d;
            ViewBag.PublisherID = new SelectList(publishersQuery, "PublisherID", "Name", selectedPublisher);
        }

        // GET: Series/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var series = db.Series.Find(id);
            if (series == null)
            {
                return HttpNotFound();
            }
            return View(series);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var series = db.Series.Find(id);
            db.Series.Remove(series);
            db.SaveChanges();
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