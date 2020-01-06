using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ImageController : Controller
    {
        private ImageDbContext db = new ImageDbContext();
        // GET: Image
        public ActionResult Index()
        {
            var images = from image in db.Images
                         orderby image.date
                         select image;
            ViewBag.Images = images;
            return View();
        }
        public ActionResult Show(int id)
        {

            Image image = db.Images.Find(id);
            ViewBag.Image = image;
            return View();
        }
        public ActionResult New(int id)
        {

            return View();
        }
        public ActionResult New(Image image)
        {

            try
            {
                db.Images.Add(image);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            Image image = db.Images.Find(id);
            ViewBag.Image = image;
            return View();
        }
        [HttpPut]
        public ActionResult Edit(int id, Image requestImage)
        {
                Image image = db.Images.Find(id);
                if(TryUpdateModel(image))
                {
                    image.name = requestImage.name;
                    image.category = requestImage.category;
                    image.name = requestImage.name;

            }
            ViewBag.Image = image;
            return View();
        }
    }
}