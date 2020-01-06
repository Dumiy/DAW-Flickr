using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "User - Registred,Administrator")]
        public ActionResult AddImage()
        {
            image Imagine = new image();
            return View(Imagine);
        }
        [HttpPost]
        [Authorize(Roles = "User - Registred,Administrator")]
        public ActionResult AddImage(image model,HttpPostedFileBase image1)
        {
                var db = new Entities();
                Console.Write(User.Identity.GetUserId().ToString());
                if (image1 != null && User.Identity.IsAuthenticated)
                {
                    model.picture = new byte[image1.ContentLength];
                    image1.InputStream.Read(model.picture, 0, image1.ContentLength);
                    model.userId = User.Identity.GetUserId().ToString();
                }
                else
                {
                    Response.Redirect(Request.RawUrl);
                }
                db.images.Add(model);
                db.SaveChanges();
                return View(model);
      
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
    }
}