using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class PostcardsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Series
        public ActionResult Index(int? selectedSeries, int? selectedPhotographer)
        {
            var series = db.Series;
            ViewBag.SelectedSeries = new SelectList(series, "SeriesId", "Title", selectedSeries);
            int seriesId = selectedSeries.GetValueOrDefault();

            var photo = db.Photographers;
            ViewBag.SelectedPhotographers = new SelectList(photo, "PhotographerId", "FullName", selectedPhotographer);
            int photoId = selectedPhotographer.GetValueOrDefault();

            var postcards = db.Postcards
                .Where(c => !selectedSeries.HasValue || c.SeriesId == seriesId)
                .Where(c => !selectedPhotographer.HasValue || c.PhotographerId == photoId)
                .OrderBy(d => d.SeriesId)
                .Include(d => d.Series)
                .Include(d => d.Photographer);

            return View(postcards.ToList());
        }

        // GET: Series/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Postcard postcard = db.Postcards.Find(id);
            if (postcard == null)
            {
                return HttpNotFound();
            }
            return View(postcard);
        }


        public ActionResult Create()
        {
            PopulateSeriesDropDownList();
            PopulatePhotographersDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Year,SeriesId,PhotographerId,ImageLink")]Postcard postcard)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Postcards.Add(postcard);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            PopulateSeriesDropDownList(postcard.SeriesId);
            PopulatePhotographersDropDownList(postcard.PhotographerId);
            return View(postcard);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Postcard postcard = db.Postcards.Find(id);
            if (postcard == null)
            {
                return HttpNotFound();
            }
            PopulateSeriesDropDownList(postcard.SeriesId);
            PopulatePhotographersDropDownList(postcard.PhotographerId);
            return View(postcard);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var postcardToUpdate = db.Postcards.Find(id);
            if (TryUpdateModel(postcardToUpdate, "", new string[] { "Title", "Year", "SeriesId", "PhotographerId", "ImageLink" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateSeriesDropDownList(postcardToUpdate.SeriesId);
            PopulatePhotographersDropDownList(postcardToUpdate.PhotographerId);

            return View(postcardToUpdate);
        }

        private void PopulateSeriesDropDownList(object selectedSeries = null)
        {
            var seriesQuery = from d in db.Series
                                   select d;
            ViewBag.SeriesID = new SelectList(seriesQuery, "SeriesID", "Title", selectedSeries);
        }

        private void PopulatePhotographersDropDownList(object selectedPhotographer = null)
        {
            var photographerQuery = from d in db.Photographers
                              select d;
            ViewBag.PhotographerId = new SelectList(photographerQuery, "PhotographerId", "FullName", selectedPhotographer);
        }

        // GET: Series/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Postcard postcard = db.Postcards.Find(id);
            if (postcard == null)
            {
                return HttpNotFound();
            }
            return View(postcard);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Postcard series = db.Postcards.Find(id);
            db.Postcards.Remove(series);
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
