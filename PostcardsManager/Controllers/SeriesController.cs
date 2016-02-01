using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PostcardsManager.Models;
using PostcardsManager.ViewModels;
using UploadcareCSharp.Url;
using PostcardsManager.Repositories;
using System;

namespace PostcardsManager.Controllers
{
    public class SeriesController : Controller
    {
        // GET: Series
        public ActionResult Index(int? selectedPublisher)
        {
            IDisposable context;
            var seriesRepository = new SeriesRepository();
            var publishersRepository = new PublisherRepository();

            var publishers = publishersRepository.GetAll(out context).ToList();
            context.Dispose();
            ViewBag.SelectedPublisher = new SelectList(publishers, "Id", "Name", selectedPublisher);
            var publisherId = selectedPublisher.GetValueOrDefault();

            var series = seriesRepository.GetAll(out context).Where(c => !selectedPublisher.HasValue || c.PublisherId == publisherId)
                .OrderBy(d => d.Id)
                .Include(d => d.Publisher);

            using (context)
            {
                return View(series.ToList());
            }
        }

        // GET: Series/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var seriesRepository = new SeriesRepository();
            var postcardsRepository = new PostcardsRepository();

            var series = seriesRepository.GetById(id);
            if (series == null)
            {
                return HttpNotFound();
            }

            var postcards = postcardsRepository.GetBySeriesId(id).OrderByDescending(p => p.Id);

            var viewModel = new SeriesViewModel
            {
                Postcards = postcards.Select(p => new PostcardMainPageViewModel
                {
                    Id = p.Id,
                    ImageFrontUrl = Urls.Cdn(new CdnPathBuilder(p.ImageFrontUniqId).Resize(360, 226)).OriginalString,
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
                    var seriesRepository = new SeriesRepository();
                    seriesRepository.Add(series);

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
        public ActionResult Edit(int id)
        {
            var seriesRepository = new SeriesRepository();

            var series = seriesRepository.GetById(id);
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
        public ActionResult EditPost(int id)
        {
            var seriesRepository = new SeriesRepository();

            var seriesToUpdate = seriesRepository.GetById(id);
            if (TryUpdateModel(seriesToUpdate, "",
                new[] { "Title", "Year", "PublisherId", "Description" }))
            {
                try
                {
                    seriesRepository.Update(seriesToUpdate);

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "[[[Unable to save changes. Try again, and if the problem persists see your system administrator.]]]");
                }
            }
            PopulatePublishersDropDownList(seriesToUpdate.PublisherId);
            return View(seriesToUpdate);
        }

        private void PopulatePublishersDropDownList(object selectedPublisher = null)
        {
            var publisherRepository = new PublisherRepository();

            IDisposable context;
            var publishersQuery = publisherRepository.GetAll(out context);

            using (context)
            {
                ViewBag.PublisherID = new SelectList(publishersQuery.ToList(), "Id", "Name", selectedPublisher);
            }
        }

        // GET: Series/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var seriesRepository = new SeriesRepository();

            var series = seriesRepository.GetById(id);
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
            var seriesRepository = new SeriesRepository();
            seriesRepository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}