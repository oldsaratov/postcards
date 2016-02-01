using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using PostcardsManager.Models;
using PostcardsManager.ViewModels;
using UploadcareCSharp.Url;
using PostcardsManager.Repositories;
using System;

namespace PostcardsManager.Controllers
{
    public class PublisherController : Controller
    {
        // GET: Publisher
        public async Task<ActionResult> Index()
        {
            var publishersRepository = new PublisherRepository();
            IDisposable context;

            var publishers = publishersRepository.GetAll(out context);

            using (context)
            {
                return View(await publishers.ToListAsync());
            }
        }

        // GET: Publisher/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var publishersRepository = new PublisherRepository();
            var postcardsRepository = new PostcardsRepository();

            var publisher = publishersRepository.GetById(id);

            if (publisher == null)
            {
                return HttpNotFound();
            }

            var publisherVm = new PublisherViewModel();
            publisherVm.Id = publisher.Id;
            publisherVm.Name = publisher.Name;
            publisherVm.Description = publisher.Description;

            publisherVm.Series = publisher.Series.Select(series => new SeriesViewModel()
            {
                Id = series.Id,
                Title = series.Title
            }).ToList();

            return View(publisherVm);
        }

        // GET: Publisher/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publisher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include = "Name, Description, SeriesId")] Publisher publisher)
        {
            var publishersRepository = new PublisherRepository();
            if (ModelState.IsValid)
            {
                publishersRepository.Add(publisher);
                return RedirectToAction("Index");
            }

            return View(publisher);
        }

        // GET: Publisher/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var publishersRepository = new PublisherRepository();

            var publisher = publishersRepository.GetById(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Publisher/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> EditPost(int id)
        {
            var publishersRepository = new PublisherRepository();

            var publisherToUpdate = publishersRepository.GetById(id);

            if (TryUpdateModel(publisherToUpdate, "",
                new[] {"Name", "Description", "SeriesId"}))
            {
                try
                {
                    publishersRepository.Update(publisherToUpdate);

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return View(publisherToUpdate);
        }

        // GET: Publisher/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int id, bool? concurrencyError)
        {
            var publishersRepository = new PublisherRepository();

            var publisher = publishersRepository.GetById(id);
            if (publisher == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }

            return View(publisher);
        }

        // POST: Publisher/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(Publisher publisher)
        {
            try
            {
                var publishersRepository = new PublisherRepository();
                publishersRepository.Delete(publisher.Id);

                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new {concurrencyError = true, id = publisher.Id});
            }
            catch (DataException /* dex */)
            {
                ModelState.AddModelError(string.Empty,
                    "[[[Delete failed. Try again, and if the problem persists see your system administrator.]]]");
                return View(publisher);
            }
        }
    }
}