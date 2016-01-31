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
        [ResponseType(typeof(IEnumerable<PostcardMainPageAPIViewModel>))]
        public HttpResponseMessage GetAll()
        {
            var postcardsRepository = new PostcardsRepository();
            
            IDisposable context = null;
            var postcards = postcardsRepository.GetAll(out context);

            using (context)
            {
                var result = postcards.Select(p => new PostcardMainPageAPIViewModel
                {
                    Id = p.Id,
                    ImageFrontUrl = Urls.Cdn(new CdnPathBuilder(p.ImageFrontUniqId).Resize(360, 226)).OriginalString,
                    FrontTitle = p.FrontTitle,
                    SeriesId = p.SeriesId,
                    Year = p.Year
                });

                return Request.CreateResponse(HttpStatusCode.OK, postcards);
            }
        }
    }
}