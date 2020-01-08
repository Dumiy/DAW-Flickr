using Microsoft.AspNet.Identity;
using PhotoSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoSharing.Controllers
{
    public class AlbumController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Album
        public ActionResult Index()
        {

            return View();
        }

        // GET: Album/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Album/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Album/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Create(Album album)
        {
           
            //    if (ModelState.IsValid)
            //    {
            //        string userId = User.Identity.GetUserId();

            //        ApplicationUser user = db.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();
            //        album.Owner = user;
            //    try
            //    {
            //        db.Albums.Add(album);
            //        db.SaveChanges();
            //        return RedirectToAction("Details/" + album.Id, "Album");
            //    } catch(Exception e)
            //    {
            //        Console.WriteLine(e.ToString());
            //    }
            //}

            return View(album);
            
        }

        // GET: Album/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Album/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Album/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Album/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
