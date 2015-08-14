using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using PostcardsManager.DAL;
using PostcardsManager.Models;
using PostcardsManager.ViewModels;
using UploadcareCSharp.API;
using UploadcareCSharp.Url;

namespace PostcardsManager.Controllers
{
    public class PostcardsController : Controller
    {
        private readonly PostcardContext db = new PostcardContext();
        // GET: Series
        public ActionResult Index(int? selectedSeries, int? selectedPhotographer)
        {
            var series = db.Series;
            ViewBag.SelectedSeries = new SelectList(series, "Id", "Title", selectedSeries);
            var seriesId = selectedSeries.GetValueOrDefault();

            var photo = db.Photographers;
            ViewBag.SelectedPhotographers = new SelectList(photo, "Id", "FullName", selectedPhotographer);
            var photoId = selectedPhotographer.GetValueOrDefault();

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
            var postcard = db.Postcards.Find(id);
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
            ViewBag.PublicKey = db.Storages.First(s => s.Enabled).PublicKey;

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
                    var storage = db.Storages.ToList().First(s => s.Enabled);
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

                        db.Images.Add(imageFront);
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

                        db.Images.Add(imageBack);
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

                    db.Postcards.Add(postcard);

                    db.SaveChanges();
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var postcard = db.Postcards.Find(id);
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

            ViewBag.PublicKey = db.Storages.First(s => s.Enabled).PublicKey;
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
            var postcardToUpdate = db.Postcards.Find(postcardVm.Id);

            try
            {
                var storage = db.Storages.ToList().First(s => s.Enabled);
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

                    db.Images.Add(imageFront);
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

                    db.Images.Add(imageBack);
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
                }
                if (imageBack != null)
                {
                    postcardToUpdate.ImageBack = imageBack;
                }

                db.SaveChanges();

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
            catch (SEHException ex)
            {
                ModelState.AddModelError("",
                    "[[[Unable to save changes. Try again, and if the problem persists, see your system administrator.]]]");
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
            var seriesQuery = from d in db.Series
                select d;


            ViewBag.SeriesId = new SelectList(seriesQuery, "Id", "Title", selectedSeries);
        }

        private void PopulatePhotographersDropDownList(object selectedPhotographer = null)
        {
            var photographerQuery = from d in db.Photographers
                select d;
            ViewBag.PhotographerId = new SelectList(photographerQuery, "Id", "FullName",
                selectedPhotographer);
        }

        // GET: Series/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var postcard = db.Postcards.Find(id);
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
            var postcard = db.Postcards.Find(id);
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

                db.Postcards.Remove(postcard);
                db.SaveChanges();

                var storage = db.Storages.ToList().First(s => s.Enabled);
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