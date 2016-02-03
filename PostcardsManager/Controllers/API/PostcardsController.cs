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
    }
}