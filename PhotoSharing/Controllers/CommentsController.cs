using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoSharing.Models;
using Microsoft.AspNet.Identity;

namespace PhotoSharing.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.Comments.ToList());
        }

        // GET: Comments/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Create([Bind(Include = "Id,Content")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                string photoIdString = Request.Form[nameof(Photo.Id)];
                int photoId = System.Convert.ToInt32(photoIdString);

                ApplicationUser user = db.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();
                Photo photo = db.Photos.Where(p => p.Id == photoId).FirstOrDefault();
                comment.Author = new ApplicationUser();
                comment.Photo = new Photo();
                comment.Photo.Owner = photo.Owner;
                comment.Author = user;
                comment.Photo = photo;
                comment.Timestamp = DateTime.Now;
                try
                {
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    return RedirectToAction("Details/"+photo.Id, "Photos");

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Edit(int id, Comment reqComment)
        {
            if (ModelState.IsValid)
            {
                 
                Comment comment = db.Comments.Find(id);
                if (comment.Author.Id == User.Identity.GetUserId() ||
                        User.IsInRole("Administrator")) {
                    comment.Content = reqComment.Content;
                db.SaveChanges();
                }
                return RedirectToAction("../Photos/Details", new { id = comment.Photo.Id });
            }
            return View(reqComment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            int idToReturn = comment.Photo.Id;
            if (comment.Author.Id == User.Identity.GetUserId() ||
                        User.IsInRole("Administrator")|| comment.Photo.Owner.Id == User.Identity.GetUserId())
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
            return RedirectToAction("../Photos/Details", new { id = idToReturn });
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
