using System.Linq;
using System.Web.Mvc;
using PostcardsManager.DAL;

namespace PostcardsManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostcardContext db = new PostcardContext();

        public ActionResult Index()
        {
            var postcards = db.Postcards.ToList();

            return View(postcards);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}