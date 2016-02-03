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
    public class SeriesController : ApiController
    {
        [ResponseType(typeof(IEnumerable<SeriesAPIViewModel>))]
        public HttpResponseMessage GetAll()
        {
            var seriesRepository = new SeriesRepository();

            IDisposable context;
            var series = seriesRepository.GetAll(out context).ToList();

            using (context)
            {
                var results = series.Select(p => new SeriesAPIViewModel(p));

                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
        }

        [ResponseType(typeof(IEnumerable<SeriesAPIViewModel>))]
        public HttpResponseMessage GetById(int id)
        {
            var seriesRepository = new SeriesRepository();

            var series = seriesRepository.GetById(id);

            return Request.CreateResponse(HttpStatusCode.OK, new SeriesAPIViewModel(series));

        }
    }
}