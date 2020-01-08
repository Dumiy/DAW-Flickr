using PhotoSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoSharing.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string category = "")
        {
            List<Photo> photos = new List<Photo>();
            if (string.IsNullOrEmpty(category) || category.Equals("all", StringComparison.InvariantCultureIgnoreCase))
                photos = db.Photos.OrderByDescending(a => a.Date).Take(10).ToList();
            else
                photos = db.Photos.Where(p => p.Category.Name.Equals(category, StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(a => a.Date).Take(10).ToList();
            return View(photos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}