using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using PostcardsManager.ViewModels;
using UploadcareCSharp.Url;
using PostcardsManager.Repositories;
using PostcardsManager.Models;
using UploadcareCSharp.API;

namespace PostcardsManager.Controllers.API
{
    [EnableCors("*", "*", "*")]
    public class PostcardsController : ApiController
    {
        private object postcard;

        [ResponseType(typeof(IEnumerable<PostcardMainPageAPIViewModel>))]
        public HttpResponseMessage GetAllForMainPage()
        {
            var postcardsRepository = new PostcardsRepository();
            
            IDisposable context = null;
            var postcards = postcardsRepository.GetAll(out context);

            using (context)
            {
                var result = postcards.ToList().Select(p => new PostcardMainPageAPIViewModel
                {
                    Id = p.Id,
                    ImageFrontUrl = Urls.Cdn(new CdnPathBuilder(p.ImageFrontUniqId).Resize(360, 226)).OriginalString,
                    FrontTitle = p.FrontTitle,
                    SeriesId = p.SeriesId,
                    PhotographerId = p.PhotographerId,
                    Year = p.Year
                });

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

        [ResponseType(typeof(IEnumerable<PostcardAPIViewModel>))]
        public HttpResponseMessage GetAll()
        {
            var postcardsRepository = new PostcardsRepository();

            IDisposable context = null;
            var postcards = postcardsRepository.GetAll(out context);

            using (context)
            {
                var result = postcards.ToList().Select(p => new PostcardAPIViewModel(p));

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

        [ResponseType(typeof(IEnumerable<PostcardAPIViewModel>))]
        public HttpResponseMessage GetById(int id)
        {
            var postcardsRepository = new PostcardsRepository();
            
            var postcard = postcardsRepository.GetById(id);

            return Request.CreateResponse(HttpStatusCode.OK, new PostcardAPIViewModel(postcard));

        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public HttpResponseMessage Add(PostcardAPIViewModel model)
        {
            var storageRepository = new StorageRepository();
            var postcardsRepository = new PostcardsRepository();

            IDisposable context;
            var storage = storageRepository.GetAll(out context).First(s => s.Enabled);
            context.Dispose();

            var client = new Client(storage.PublicKey, storage.PrivateKey);

            Image imageFront = null;
            if (model.ImageFrontUrl != null)
            {
                var imageId = GetUniqIdFromUrl(model.ImageFrontUrl);
                imageFront = new Image
                {
                    StorageId = storage.Id,
                    Url = model.ImageFrontUrl,
                    UniqImageId = imageId
                };

                postcardsRepository.AddImage(imageFront);
                client.SaveFile(imageId);
            }

            Image imageBack = null;
            if (model.ImageBackUrl != null)
            {
                var imageId = GetUniqIdFromUrl(model.ImageBackUrl);
                imageBack = new Image
                {
                    StorageId = storage.Id,
                    Url = model.ImageBackUrl,
                    UniqImageId = GetUniqIdFromUrl(model.ImageBackUrl)
                };

                postcardsRepository.AddImage(imageBack);
                client.SaveFile(imageId);
            }

            var postcard = new Postcard
            {
                Year = model.Year,
                PhotographerId = model.PhotographerId,
                SeriesId = model.SeriesId,
                FrontTitle = model.FrontTitle,
                FrontTitleFont = model.FrontTitleFont,
                FrontTitleFontColor = model.FrontTitleFontColor,
                BackTitle = model.BackTitle,
                BackTitleFont = model.BackTitleFont,
                BackTitleFontColor = model.BackTitleFontColor,
                BackTitlePlace = model.BackTitlePlace,
                BackType = model.BackType,
                NumberInSeries = model.NumberInSeries,
                PostDate = model.PostDate,
                PublishPlace = model.PublishPlace
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

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public HttpResponseMessage Update(PostcardAPIViewModel model)
        {
            if (model.Id == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var postcardsRepository = new PostcardsRepository();
            var postcardToUpdate = postcardsRepository.GetById(model.Id);

            var storageRepository = new StorageRepository();

            IDisposable context;
            var storage = storageRepository.GetAll(out context).First(s => s.Enabled);
            context.Dispose();

            var client = new Client(storage.PublicKey, storage.PrivateKey);

            var oldFrontImage = postcardToUpdate.ImageFrontUniqId;
            var oldBackImage = postcardToUpdate.ImageBackUniqId;

            Image imageFront = null;
            if (model.ImageFrontUrl != null)
            {
                var imageId = GetUniqIdFromUrl(model.ImageFrontUrl);
                imageFront = new Image
                {
                    StorageId = storage.Id,
                    Url = model.ImageFrontUrl,
                    UniqImageId = imageId
                };

                postcardsRepository.AddImage(imageFront);
                client.SaveFile(imageId);
            }

            Image imageBack = null;
            if (model.ImageBackUrl != null)
            {
                var imageId = GetUniqIdFromUrl(model.ImageBackUrl);
                imageBack = new Image
                {
                    StorageId = storage.Id,
                    Url = model.ImageBackUrl,
                    UniqImageId = imageId
                };

                postcardsRepository.AddImage(imageBack);
                client.SaveFile(imageId);
            }

            postcardToUpdate.Year = model.Year;
            postcardToUpdate.PhotographerId = model.PhotographerId;
            postcardToUpdate.SeriesId = model.SeriesId;
            postcardToUpdate.FrontTitle = model.FrontTitle;
            postcardToUpdate.FrontTitleFont = model.FrontTitleFont;
            postcardToUpdate.FrontTitleFontColor = model.FrontTitleFontColor;
            postcardToUpdate.BackTitle = model.BackTitle;
            postcardToUpdate.BackTitleFont = model.BackTitleFont;
            postcardToUpdate.BackTitleFontColor = model.BackTitleFontColor;
            postcardToUpdate.BackTitlePlace = model.BackTitlePlace;
            postcardToUpdate.BackType = model.BackType;
            postcardToUpdate.NumberInSeries = model.NumberInSeries;
            postcardToUpdate.PostDate = model.PostDate;
            postcardToUpdate.PublishPlace = model.PublishPlace;

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

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        private static Guid GetUniqIdFromUrl(string url)
        {
            var lastPart = url.Split('/').Last(l => l.Length > 0);

            return new Guid(lastPart);
        }
    }
}