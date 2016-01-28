using PostcardsManager.DAL;
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

namespace PostcardsManager.Controllers.API
{
    [EnableCors("*", "*", "*")]
    public class PostcardsController : ApiController
    {
        private readonly PostcardContext db = new PostcardContext();
        
        [ResponseType(typeof(IEnumerable<PostcardMainPageViewModel>))]
        public HttpResponseMessage GetAll()
        {
            var postcards =
                db.Postcards.OrderByDescending(p => p.Id).Take(50).ToList().Select(p => new PostcardMainPageViewModel
                {
                    Id = p.Id,
                    ImageFrontUrl = Urls.Cdn(new CdnPathBuilder(p.ImageFrontUniqId).Resize(360, 226)).OriginalString,
                    FrontTitle = p.FrontTitle
                });

            return Request.CreateResponse(HttpStatusCode.OK, postcards);
        }
    }
}