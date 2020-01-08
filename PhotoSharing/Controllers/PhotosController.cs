using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoSharing.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace PhotoSharing.Controllers
{
    public class PhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Photos
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.Photos.ToList());
        }

        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhotoId = id;
            return View(photo);
        }

        // GET: Photos/Create
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Create(string Title, HttpPostedFileBase file, int category)
        {
            var photo = new Photo();
            if (file != null && file.ContentLength > 0)
                try
                {
                    var guid = Guid.NewGuid();
                    var extension = file.FileName.Split('.').Last();
                    string fileName = guid.ToString() + '.' + extension;

                    string path = Path.Combine(Server.MapPath("~/Images"),
                                               Path.GetFileName(fileName));
                    file.SaveAs(path);
                    photo.URL = $"~/Images/{Path.GetFileName(fileName)}";
                    photo.Title = Title;
                    photo.Category = db.Categories.Where(c => c.Id == category).FirstOrDefault();
                    string userId = User.Identity.GetUserId();
                    photo.Owner = db.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();
                    photo.Date = DateTime.Now;

                    db.Photos.Add(photo);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }

            return RedirectToAction("Details", new { id = photo.Id });
        }

        // GET: Photos/Edit/5
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Edit(int id, Photo reqPhoto)
        {
            if (ModelState.IsValid)
            {
                Photo photo = db.Photos.Find(id);
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();

                if (photo.Owner.Id == User.Identity.GetUserId() ||
                       User.IsInRole("Administrator"))
                {
                    if (TryUpdateModel(photo))
                    {
                        photo.Title = reqPhoto.Title;
                        photo.URL = reqPhoto.URL;
                        photo.Category = reqPhoto.Category;
                        db.SaveChanges();
                        TempData["message"] = "Poza a fost modificata!";
                    }
                    return RedirectToAction("Details", new { id = photo.Id });
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unuei poze care nu va apartine!";
                    return RedirectToAction("Details", new { id = photo.Id });
                }
            }
            else
            {
                return View(reqPhoto);
            }
            
        }

        // GET: Photos/Delete/5
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photos.Find(id);

            foreach(Comment comment in photo.Comments.ToList())
            {
                db.Comments.Remove(comment);
            }

            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("../Home");
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
