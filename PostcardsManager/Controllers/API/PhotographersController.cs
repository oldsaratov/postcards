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
                var results = photographers.Select(p => new PhotographerAPIViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName
                });

                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
        }
    }
}