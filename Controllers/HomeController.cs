using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using PostcardsManager.DAL;
using PostcardsManager.ViewModels;
using UploadcareCSharp.Url;

namespace PostcardsManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostcardContext db = new PostcardContext();

        public ActionResult Index()
        {
            var postcards = db.Postcards.OrderByDescending(p => p.Id).Take(50).ToList().Select(p => new PostcardMainPageViewModel
            {
                Id = p.Id,
                ImageFrontUrl = Urls.Cdn(new CdnPathBuilder(p.ImageFrontUniqId).Resize(360, 226)).OriginalString,
                FrontTitle = p.FrontTitle
            });

            return View(postcards);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}