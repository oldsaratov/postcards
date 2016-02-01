using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using PostcardsManager.ViewModels;
using PostcardsManager.Repositories;

namespace PostcardsManager.Controllers.API
{
    [EnableCors("*", "*", "*")]
    public class PublisherController : ApiController
    {
        [ResponseType(typeof(IEnumerable<PublisherViewModel>))]
        public HttpResponseMessage GetAll()
        {
            var publishersRepository = new PublisherRepository();

            IDisposable context;
            var publishers = publishersRepository.GetAll(out context).OrderByDescending(p => p.Id).ToList();

            using (context)
            {
                var results = publishers.Select(p => new PublisherViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                });

                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
        }
    }
}