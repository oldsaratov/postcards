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
    public class PhotographersController : ApiController
    {
        [ResponseType(typeof(IEnumerable<PhotographerAPIViewModel>))]
        public HttpResponseMessage GetAll()
        {
            var photographerRepository = new PhotographerRepository();

            IDisposable context;
            var photographers = photographerRepository.GetAll(out context).ToList();

            using (context)
            {
                var results = photographers.Select(p => new PhotographerAPIViewModel(p));

                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
        }

        [ResponseType(typeof(IEnumerable<PhotographerAPIViewModel>))]
        public HttpResponseMessage GetById(int id)
        {
            var photographerRepository = new PhotographerRepository();

            var photographer = photographerRepository.GetById(id);

            return Request.CreateResponse(HttpStatusCode.OK, new PhotographerAPIViewModel(photographer));

        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public HttpResponseMessage Add(PhotographerAPIViewModel model)
        {
            var photographerRepository = new PhotographerRepository();

            var newPhotographer = new Photographer()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName
            };

            photographerRepository.Add(newPhotographer);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public HttpResponseMessage Update(PhotographerAPIViewModel model)
        {
            var photographerRepository = new PhotographerRepository();

            var updatedPhotographer = new Photographer
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName
            };

            photographerRepository.Update(updatedPhotographer);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}