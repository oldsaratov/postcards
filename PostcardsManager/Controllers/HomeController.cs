using System.Linq;
using System.Web.Mvc;
using PostcardsManager.ViewModels;
using UploadcareCSharp.Url;
using PostcardsManager.Repositories;
using System;

namespace PostcardsManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var postcardsRepository = new PostcardsRepository();

            IDisposable context = null;
            var postcards = postcardsRepository.GetAll(out context);

            using (context)
            {
                var result = postcards.OrderByDescending(p => p.Id).Take(50).ToList().Select(p => new PostcardMainPageViewModel
                {
                    Id = p.Id,
                    ImageFrontUrl = Urls.Cdn(new CdnPathBuilder(p.ImageFrontUniqId).Resize(360, 226)).OriginalString,
                    FrontTitle = p.FrontTitle
                });

                return View(result);
            }
        }
    }
}