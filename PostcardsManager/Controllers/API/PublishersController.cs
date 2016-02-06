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
using PostcardsManager.Models;

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

        [HttpPost]
        [ResponseType(typeof(void))]
        public HttpResponseMessage Add(PublisherAPIViewModel model)
        {
            var publishersRepository = new PublisherRepository();

            var newPublisher = new Publisher()
            {
                Name = model.Name,
                Description = model.Description
            };

            publishersRepository.Add(newPublisher);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public HttpResponseMessage Update(PublisherAPIViewModel model)
        {
            var publishersRepository = new PublisherRepository();

            var updatedPublisher = new Publisher
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            publishersRepository.Update(updatedPublisher);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}