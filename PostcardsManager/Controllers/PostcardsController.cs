using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PostcardsManager.Models;
using PostcardsManager.ViewModels;
using UploadcareCSharp.API;
using UploadcareCSharp.Url;
using PostcardsManager.Repositories;

namespace PostcardsManager.Controllers
{
    public class PostcardsController : Controller
    {
        // GET: Series
        public ActionResult Index(int? selectedSeries, int? selectedPhotographer)
        {
            var postcardsRepository = new PostcardsRepository();
            var seriesRepository = new SeriesRepository();
            var photographerRepository = new PhotographerRepository();

            IDisposable context = null;
            var postcards = postcardsRepository.GetAll(out context);

            using (context)
            {
                IDisposable context1;
                var series = seriesRepository.GetAll(out context1).ToList();
                context1.Dispose();
                ViewBag.SelectedSeries = new SelectList(series, "Id", "Title", selectedSeries);
                var seriesId = selectedSeries.GetValueOrDefault();

                IDisposable context2;
                var photo = photographerRepository.GetAll(out context2).ToList();
                context2.Dispose();
                ViewBag.SelectedPhotographers = new SelectList(photo, "Id", "FullName", selectedPhotographer);
                var photoId = selectedPhotographer.GetValueOrDefault();

                var result = postcards
                    .Where(c => !selectedSeries.HasValue || c.SeriesId == seriesId)
                    .Where(c => !selectedPhotographer.HasValue || c.PhotographerId == photoId)
                    .OrderBy(d => d.SeriesId);

                return View(result.ToList());
            }
        }

        // GET: Series/Details/5
        public ActionResult Details(int id)
        {
            var postcardsRepository = new PostcardsRepository();

            var postcard = postcardsRepository.GetById(id);
            if (postcard == null)
            {
                return HttpNotFound();
            }

            var postcardVm = new PostcardViewModel
            {
                Id = postcard.Id,
                ImageFrontId = postcard.ImageFrontId,
                ImageBackId = postcard.ImageBackId,
                Year = postcard.Year,
                FrontTitle = postcard.FrontTitle,
                FrontTitleFont = postcard.FrontTitleFont,
                FrontTitleFontColor = postcard.FrontTitleFontColor,
                BackTitle = postcard.BackTitle,
                BackTitleFont = postcard.BackTitleFont,
                BackTitleFontColor = postcard.BackTitleFontColor,
                BackType = postcard.BackType,
                BackTitlePlace = postcard.BackTitlePlace,
                NumberInSeries = postcard.NumberInSeries,
                PostDate = postcard.PostDate,
                PublishPlace = postcard.PublishPlace
            };

            if (postcard.ImageFront != null)
            {
                postcardVm.ImageFrontPreviewUrl =
                    Urls.Cdn(new CdnPathBuilder(postcard.ImageFrontUniqId).ResizeHeight(400)).OriginalString;
                postcardVm.ImageFrontFullUrl =
                    Urls.Cdn(new CdnPathBuilder(postcard.ImageFrontUniqId).ResizeHeight(1000)).OriginalString;
            }

            if (postcard.ImageBack != null)
            {
                postcardVm.ImageBackPreviewUrl =
                    Urls.Cdn(new CdnPathBuilder(postcard.ImageBackUniqId).ResizeHeight(400)).OriginalString;
                postcardVm.ImageBackFullUrl =
                    Urls.Cdn(new CdnPathBuilder(postcard.ImageBackUniqId).ResizeHeight(1000)).OriginalString;
            }

            if (postcard.SeriesId.HasValue)
            {
                postcardVm.SeriesId = postcard.SeriesId;
                postcardVm.SeriesTitle = postcard.Series.Title;
            }

            if (postcard.PhotographerId.HasValue)
            {
                postcardVm.PhotographerId = postcard.PhotographerId;
                postcardVm.PhotographerName = postcard.Photographer.FullName;
            }

            return View(postcardVm);
        }

        [Authorize]
        public ActionResult Create()
        {
            var storageRepository = new StorageRepository();

            IDisposable context;
            ViewBag.PublicKey = storageRepository.GetAll(out context).First(s => s.Enabled).PublicKey;
            context.Dispose();

            PopulateSeriesDropDownList();
            PopulatePhotographersDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(
            [Bind(
                Include =
                    "Year, PhotographerId, SeriesId, ImageFrontUrl, FrontTitle, FrontTitleFont, FrontTitleFontColor, ImageBackUrl, BackTitle, BackTitleFont, BackTitleFontColor, BackTitlePlace, BackType, NumberInSeries, PostDate, PublishPlace"
                )] PostcardEditViewModel postcardVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var storageRepository = new StorageRepository();
                    var postcardsRepository = new PostcardsRepository();

                    IDisposable context;
                    var storage = storageRepository.GetAll(out context).First(s => s.Enabled);
                    context.Dispose();
                    
                    var client = new Client(storage.PublicKey, storage.PrivateKey);

                    Image imageFront = null;
                    if (postcardVm.ImageFrontUrl != null)
                    {
                        var imageId = GetUniqIdFromUrl(postcardVm.ImageFrontUrl);
                        imageFront = new Image
                        {
                            StorageId = storage.Id,
                            Url = postcardVm.ImageFrontUrl,
                            UniqImageId = imageId
                        };

                        postcardsRepository.AddImage(imageFront);
                        client.SaveFile(imageId);
                    }

                    Image imageBack = null;
                    if (postcardVm.ImageBackUrl != null)
                    {
                        var imageId = GetUniqIdFromUrl(postcardVm.ImageBackUrl);
                        imageBack = new Image
                        {
                            StorageId = storage.Id,
                            Url = postcardVm.ImageBackUrl,
                            UniqImageId = GetUniqIdFromUrl(postcardVm.ImageBackUrl)
                        };

                        postcardsRepository.AddImage(imageBack);
                        client.SaveFile(imageId);
                    }

                    var postcard = new Postcard
                    {
                        Year = postcardVm.Year,
                        PhotographerId = postcardVm.PhotographerId,
                        SeriesId = postcardVm.SeriesId,
                        FrontTitle = postcardVm.FrontTitle,
                        FrontTitleFont = postcardVm.FrontTitleFont,
                        FrontTitleFontColor = postcardVm.FrontTitleFontColor,
                        BackTitle = postcardVm.BackTitle,
                        BackTitleFont = postcardVm.BackTitleFont,
                        BackTitleFontColor = postcardVm.BackTitleFontColor,
                        BackTitlePlace = postcardVm.BackTitlePlace,
                        BackType = postcardVm.BackType,
                        NumberInSeries = postcardVm.NumberInSeries,
                        PostDate = postcardVm.PostDate,
                        PublishPlace = postcardVm.PublishPlace
                    };

                    if (imageFront != null)
                    {
                        postcard.ImageFront = imageFront;
                    }
                    if (imageFront != null)
                    {
                        postcard.ImageBack = imageBack;
                    }
                    
                    postcardsRepository.Add(postcard);

                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("",
                    "[[[Unable to save changes. Try again, and if the problem persists, see your system administrator.]]]");
            }

            PopulateSeriesDropDownList(postcardVm.SeriesId);
            PopulatePhotographersDropDownList(postcardVm.PhotographerId);
            return View(postcardVm);
        }

        private static Guid GetUniqIdFromUrl(string url)
        {
            var lastPart = url.Split('/').Last(l => l.Length > 0);

            return new Guid(lastPart);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var postcardsRepository = new PostcardsRepository();
            var postcard = postcardsRepository.GetById(id);
            if (postcard == null)
            {
                return HttpNotFound();
            }

            var postcardVm = new PostcardEditViewModel
            {
                Id = postcard.Id,
                Year = postcard.Year,
                PhotographerId = postcard.PhotographerId,
                SeriesId = postcard.SeriesId,
                FrontTitle = postcard.FrontTitle,
                FrontTitleFont = postcard.FrontTitleFont,
                FrontTitleFontColor = postcard.FrontTitleFontColor,
                BackTitle = postcard.BackTitle,
                BackTitleFont = postcard.BackTitleFont,
                BackTitleFontColor = postcard.BackTitleFontColor,
                BackTitlePlace = postcard.BackTitlePlace,
                BackType = postcard.BackType,
                NumberInSeries = postcard.NumberInSeries,
                PostDate = postcard.PostDate,
                PublishPlace = postcard.PublishPlace
            };

            if (postcard.ImageFront != null)
                postcardVm.ImageFrontUrl = postcard.ImageFront.UniqImageId.ToString();

            if (postcard.ImageBack != null)
                postcardVm.ImageBackUrl = postcard.ImageBack.UniqImageId.ToString();

            var storageRepository = new StorageRepository();

            IDisposable context;
            ViewBag.PublicKey = storageRepository.GetAll(out context).First(s => s.Enabled).PublicKey;
            context.Dispose();

            PopulateSeriesDropDownList(postcard.SeriesId);
            PopulatePhotographersDropDownList(postcard.PhotographerId);

            return View(postcardVm);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditPost(
            [Bind(
                Include =
                    "Id, Year, PhotographerId, SeriesId, ImageFrontUrl, FrontTitle, FrontTitleFont, FrontTitleFontColor, ImageBackUrl, BackTitle, BackTitleFont, BackTitleFontColor, BackTitlePlace, BackType, NumberInSeries, PostDate, PublishPlace"
                )] PostcardEditViewModel postcardVm)
        {
            if (postcardVm.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var postcardsRepository = new PostcardsRepository();
            var postcardToUpdate = postcardsRepository.GetById(postcardVm.Id);

            try
            {
                var storageRepository = new StorageRepository();

                IDisposable context;
                var storage = storageRepository.GetAll(out context).First(s => s.Enabled);
                context.Dispose();

                var client = new Client(storage.PublicKey, storage.PrivateKey);

                var oldFrontImage = postcardToUpdate.ImageFrontUniqId;
                var oldBackImage = postcardToUpdate.ImageBackUniqId;

                Image imageFront = null;
                if (postcardVm.ImageFrontUrl != null)
                {
                    var imageId = GetUniqIdFromUrl(postcardVm.ImageFrontUrl);
                    imageFront = new Image
                    {
                        StorageId = storage.Id,
                        Url = postcardVm.ImageFrontUrl,
                        UniqImageId = imageId
                    };

                    postcardsRepository.AddImage(imageFront);
                    client.SaveFile(imageId);
                }

                Image imageBack = null;
                if (postcardVm.ImageBackUrl != null)
                {
                    var imageId = GetUniqIdFromUrl(postcardVm.ImageBackUrl);
                    imageBack = new Image
                    {
                        StorageId = storage.Id,
                        Url = postcardVm.ImageBackUrl,
                        UniqImageId = imageId
                    };

                    postcardsRepository.AddImage(imageBack);
                    client.SaveFile(imageId);
                }

                postcardToUpdate.Year = postcardVm.Year;
                postcardToUpdate.PhotographerId = postcardVm.PhotographerId;
                postcardToUpdate.SeriesId = postcardVm.SeriesId;
                postcardToUpdate.FrontTitle = postcardVm.FrontTitle;
                postcardToUpdate.FrontTitleFont = postcardVm.FrontTitleFont;
                postcardToUpdate.FrontTitleFontColor = postcardVm.FrontTitleFontColor;
                postcardToUpdate.BackTitle = postcardVm.BackTitle;
                postcardToUpdate.BackTitleFont = postcardVm.BackTitleFont;
                postcardToUpdate.BackTitleFontColor = postcardVm.BackTitleFontColor;
                postcardToUpdate.BackTitlePlace = postcardVm.BackTitlePlace;
                postcardToUpdate.BackType = postcardVm.BackType;
                postcardToUpdate.NumberInSeries = postcardVm.NumberInSeries;
                postcardToUpdate.PostDate = postcardVm.PostDate;
                postcardToUpdate.PublishPlace = postcardVm.PublishPlace;

                if (imageFront != null)
                {
                    postcardToUpdate.ImageFront = imageFront;
                    postcardToUpdate.ImageFrontId = imageFront.Id;
                }
                if (imageBack != null)
                {
                    postcardToUpdate.ImageBack = imageBack;
                    postcardToUpdate.ImageBackId = imageBack.Id;
                }

                postcardsRepository.Update(postcardToUpdate);

                if (imageFront != null)
                {
                    if (oldFrontImage != Guid.Empty && oldFrontImage != postcardToUpdate.ImageFront.UniqImageId)
                        client.DeleteFile(oldFrontImage);
                }
                else
                {
                    if (oldFrontImage != Guid.Empty)
                        client.DeleteFile(oldFrontImage);
                }

                if (imageBack != null)
                {
                    if (oldBackImage != Guid.Empty && oldBackImage != postcardToUpdate.ImageBack.UniqImageId)
                        client.DeleteFile(oldBackImage);
                }
                else
                {
                    if (oldBackImage != Guid.Empty)
                        client.DeleteFile(oldBackImage);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",
                    "[[[Unable to save changes. Try again, and if the problem persists, see your system administrator.]]]");
            }

            PopulateSeriesDropDownList(postcardToUpdate.SeriesId);
            PopulatePhotographersDropDownList(postcardToUpdate.PhotographerId);

            return View(postcardVm);
        }

        private void PopulateSeriesDropDownList(object selectedSeries = null)
        {
            var seriesRepository = new SeriesRepository();

            IDisposable context;
            var seriesQuery = seriesRepository.GetAll(out context);

            using (context)
            {
                ViewBag.SeriesId = new SelectList(seriesQuery.ToList(), "Id", "Title", selectedSeries);
            }
        }

        private void PopulatePhotographersDropDownList(object selectedPhotographer = null)
        {

            var photographersRepository = new PhotographerRepository();

            IDisposable context;
            var photographerQuery = photographersRepository.GetAll(out context);

            using (context)
            {
                ViewBag.PhotographerId = new SelectList(photographerQuery.ToList(), "Id", "FullName",
                    selectedPhotographer);
            }
        }

        // GET: Series/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var postcardsRepository = new PostcardsRepository();
            var postcard = postcardsRepository.GetById(id);

            if (postcard == null)
            {
                return HttpNotFound();
            }
            return View(postcard);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            var postcardsRepository = new PostcardsRepository();
            var postcard = postcardsRepository.GetById(id);

            try
            {
                var imageFrontId = Guid.Empty;
                if (postcard.ImageFront != null)
                {
                    imageFrontId = postcard.ImageFront.UniqImageId;
                }
                
                var imageBackId = Guid.Empty;
                if (postcard.ImageBack != null)
                {
                    imageBackId = postcard.ImageBack.UniqImageId;
                }

                postcardsRepository.Delete(id);

                IDisposable context;
                var storageRepository = new StorageRepository();
                var storage = storageRepository.GetAll(out context).First(s => s.Enabled);
                context.Dispose();

                var client = new Client(storage.PublicKey, storage.PrivateKey);

                if (imageFrontId != Guid.Empty)
                    client.DeleteFile(imageFrontId);

                if (imageBackId != Guid.Empty)
                    client.DeleteFile(imageBackId);
            }
            catch (Exception)
            {
                ModelState.AddModelError("",
                    "[[[Unable to delete. Try again, and if the problem persists, see your system administrator.]]]");
            }
            return RedirectToAction("Index");
        }
    }
}