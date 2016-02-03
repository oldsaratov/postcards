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
    public class PublishersController : ApiController
    {
        [ResponseType(typeof(IEnumerable<PublisherAPIViewModel>))]
        public HttpResponseMessage GetAll()
        {
            var publishersRepository = new PublisherRepository();

            IDisposable context;
            var publishers = publishersRepository.GetAll(out context).ToList();

            using (context)
            {
                var results = publishers.Select(p => new PublisherAPIViewModel(p));

                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
        }

        [ResponseType(typeof(IEnumerable<PublisherAPIViewModel>))]
        public HttpResponseMessage GetById(int id)
        {
            var publishersRepository = new PublisherRepository();

            var publisher = publishersRepository.GetById(id);

            return Request.CreateResponse(HttpStatusCode.OK, new PublisherAPIViewModel(publisher));

        }
    }
}