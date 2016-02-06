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

        [HttpPost]
        [ResponseType(typeof(void))]
        public HttpResponseMessage Add(SeriesAPIViewModel model)
        {
            var seriesRepository = new SeriesRepository();

            var newSeries = new Series()
            {
                PublisherId = model.PublisherId,
                Title = model.Title,
                Year = model.Year,
                Description = model.Description
            };

            seriesRepository.Add(newSeries);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public HttpResponseMessage Update(SeriesAPIViewModel model)
        {
            var seriesRepository = new SeriesRepository();

            var updatedSeries = new Series
            {
                Id = model.Id,
                PublisherId = model.PublisherId,
                Title = model.Title,
                Year = model.Year,
                Description = model.Description
            };

            seriesRepository.Update(updatedSeries);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}