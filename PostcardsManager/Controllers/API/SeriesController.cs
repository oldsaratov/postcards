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
    public class SeriesController : ApiController
    {
        private readonly PostcardContext db = new PostcardContext();
        
        [ResponseType(typeof(IEnumerable<SeriesViewModel>))]
        public HttpResponseMessage GetAll()
        {
            var series =
                db.Series.OrderByDescending(p => p.Id).ToList().Select(p => new SeriesViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    PublisherName = p.Publisher?.Name,
                    Year = p.Year
                });

            return Request.CreateResponse(HttpStatusCode.OK, series);
        }
    }
}