using System.Web.Mvc;
using PostcardsManager.DAL;

namespace PostcardsManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        public ActionResult Index()
        {
            var postcards = db.Postcards;

            return View(postcards);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}