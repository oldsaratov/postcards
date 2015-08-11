using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PostcardsManager.DAL;
using PostcardsManager.Models;
using PostcardsManager.ViewModels;
using UploadcareCSharp.Url;

namespace PostcardsManager.Controllers
{
    public class SeriesController : Controller
    {
        private readonly PostcardContext db = new PostcardContext();
        // GET: Series
        public ActionResult Index(int? selectedPublisher)
        {
            var publishers = db.Publishers;
            ViewBag.SelectedPublisher = new SelectList(publishers, "Id", "Name", selectedPublisher);
            var publisherId = selectedPublisher.GetValueOrDefault();

            var series = db.Series
                .Where(c => !selectedPublisher.HasValue || c.PublisherId == publisherId)
                .OrderBy(d => d.Id)
                .Include(d => d.Publisher);

            return View(series.ToList());
        }

        // GET: Series/Details/5
        [Authorize]
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

            var viewModel = new SeriesViewModel
            {
                Postcards =
                    db.Postcards.OrderByDescending(p => p.Id)
                        .Take(50)
                        .ToList()
                        .Select(p => new PostcardMainPageViewModel
                        {
                            Id = p.Id,
                            ImageFrontUrl =
                                Urls.Cdn(new CdnPathBuilder(p.ImageFrontUniqId).Resize(360, 226)).OriginalString,
                            FrontTitle = p.FrontTitle
                        }).ToList(),
                Title = series.Title,
                Description = series.Description,
                Year = series.Year,
                Id = series.Id,
                PublisherName = series.Publisher != null ? series.Publisher.Name : string.Empty
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            PopulatePublishersDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "SeriesId,Title,Year,PublisherId,Description")] Series series)
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
                ModelState.AddModelError("", "[[[Unable to save changes. Try again, and if the problem persists, see your system administrator.]]]");
            }
            PopulatePublishersDropDownList(series.PublisherId);
            return View(series);
        }

        [Authorize]
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
        [Authorize]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseToUpdate = db.Series.Find(id);
            if (TryUpdateModel(courseToUpdate, "",
                new[] { "Title", "Year", "PublisherId", "Description" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "[[[Unable to save changes. Try again, and if the problem persists see your system administrator.]]]");
                }
            }
            PopulatePublishersDropDownList(courseToUpdate.PublisherId);
            return View(courseToUpdate);
        }

        private void PopulatePublishersDropDownList(object selectedPublisher = null)
        {
            var publishersQuery = from d in db.Publishers
                select d;
            ViewBag.PublisherID = new SelectList(publishersQuery, "Id", "Name", selectedPublisher);
        }

        // GET: Series/Delete/5
        [Authorize]
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
        [Authorize]
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