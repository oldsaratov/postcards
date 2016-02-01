using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PostcardsManager.Models;
using PostcardsManager.Repositories;
using System;

namespace PostcardsManager.Controllers
{
    public class StoragesController : Controller
    {
        // GET: Storages
        [Authorize]
        public ActionResult Index()
        {
            IDisposable context;

            var storageRepository = new StorageRepository();
            var storages = storageRepository.GetAll(out context);

            using (context)
            {
                return View(storages.ToList());
            }
        }

        // GET: Storages/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Storages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,StorageName,PublicKey,PrivateKey,StorageLimit")] Storage storage)
        {
            if (ModelState.IsValid)
            {
                var storageRepository = new StorageRepository();
                storageRepository.Add(storage);

                return RedirectToAction("Index");
            }

            return View(storage);
        }

        // GET: Storages/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var storageRepository = new StorageRepository();

            Storage storage = storageRepository.GetById(id);
            if (storage == null)
            {
                return HttpNotFound();
            }
            return View(storage);
        }

        // POST: Storages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,StorageName,PublicKey,PrivateKey,StorageLimit,Enabled")] Storage storage)
        {
            if (ModelState.IsValid)
            {
                var storageRepository = new StorageRepository();

                storageRepository.Update(storage);

                return RedirectToAction("Index");
            }
            return View(storage);
        }

        // GET: Storages/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var storageRepository = new StorageRepository();

            Storage storage = storageRepository.GetById(id);

            if (storage == null)
            {
                return HttpNotFound();
            }
            return View(storage);
        }

        // POST: Storages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            var storageRepository = new StorageRepository();
            storageRepository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
