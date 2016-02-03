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
    }
}