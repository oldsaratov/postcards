﻿using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using PostcardsManager.DAL;
using PostcardsManager.Models;

namespace PostcardsManager.Controllers
{
    public class PublisherController : Controller
    {
        private readonly PostcardContext db = new PostcardContext();
        // GET: Publisher
        public async Task<ActionResult> Index()
        {
            var publishers = db.Publishers;
            return View(await publishers.ToListAsync());
        }

        // GET: Publisher/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var publisher = db.Publishers.Find(id);

            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
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
            if (ModelState.IsValid)
            {
                db.Publishers.Add(publisher);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(publisher);
        }

        // GET: Publisher/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var publisher = await db.Publishers.FindAsync(id);
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
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var publisherToUpdate = db.Publishers.Find(id);
            if (TryUpdateModel(publisherToUpdate, "",
                new[] {"Name", "Description", "SeriesId"}))
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

            return View(publisherToUpdate);
        }

        // GET: Publisher/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var publisher = await db.Publishers.FindAsync(id);
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
                db.Entry(publisher).State = EntityState.Deleted;
                await db.SaveChangesAsync();
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