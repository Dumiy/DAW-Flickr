using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using Microsoft.Security.Application;

namespace WebApplication2.Controllers
{
    public class PhotoFlickrController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private int _perPage = 3;
        // GET: PhotoFlickr
        public ActionResult Index()
        {
            var postsFlickr = db.Photos.Include("Category").Include("User").OrderBy(a => a.Date);
            var totalItems = postsFlickr.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }

            var paginatedArticles = postsFlickr.Skip(offset).Take(this._perPage);

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            ViewBag.perPage = this._perPage;
            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Articles = paginatedArticles;

            return View();
        }

        // GET: PhotoFlickr/Details/5
        public ActionResult Details(int id)
        {
            PhotoFlickr photoFlickr = db.Photos.Find(id);

            ViewBag.afisareButoane = false;
            if (User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();


            return View(photoFlickr);
        }

        // GET: PhotoFlickr/Create
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Create()
        {
            
            PhotoFlickr photoPost = new PhotoFlickr();

            // preluam lista de categorii din metoda GetAllCategories()
            photoPost.Categories = GetAllCategories();

            // Preluam ID-ul utilizatorului curent
            photoPost.UserId = User.Identity.GetUserId();


            return View(photoPost);
        }

        // POST: PhotoFlickr/Create
        [HttpPost]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Create(PhotoFlickr photoFlickr)
        {
            photoFlickr.Categories = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    // Protect content from XSS
                    photoFlickr.Description = Sanitizer.GetSafeHtmlFragment(photoFlickr.Description);
                    db.Photos.Add(photoFlickr);
                    db.SaveChanges();
                    TempData["message"] = "Poza a fost adaugata!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(photoFlickr);
                }
            }
            catch (Exception e)
            {
                return View(photoFlickr);
            }
        }

        // GET: PhotoFlickr/Edit/5
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Edit(int id)
        {
            PhotoFlickr photoFlickr = db.Photos.Find(id);
            ViewBag.Photo = photoFlickr;
            photoFlickr.Categories = GetAllCategories();

            if (photoFlickr.UserId == User.Identity.GetUserId() ||
                User.IsInRole("Administrator"))
            {
                return View(photoFlickr);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Index");
            }

        }

        // POST: PhotoFlickr/Edit/5
        [HttpPut]
        [Authorize(Roles = "User,Administrator")]
        [ValidateInput(false)]
        public ActionResult Edit(int id, PhotoFlickr requestPhoto)
        {
            requestPhoto.Categories = GetAllCategories();

            try
            {
                if (ModelState.IsValid)
                {
                    PhotoFlickr photoFlickr = db.Photos.Find(id);
                    if (photoFlickr.UserId == User.Identity.GetUserId() ||
                        User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(photoFlickr))
                        {
                            photoFlickr.Title = requestPhoto.Title;
                            // Protect content from XSS
                            requestPhoto.Description = Sanitizer.GetSafeHtmlFragment(requestPhoto.Description);
                            photoFlickr.Description = requestPhoto.Description;
                            photoFlickr.Date = requestPhoto.Date;
                            photoFlickr.CategoryId = requestPhoto.CategoryId;
                            photoFlickr.Comment = requestPhoto.Comment;
                            photoFlickr.Photo = requestPhoto.Photo;
                            photoFlickr.Title = photoFlickr.Title;
                            db.SaveChanges();
                            TempData["message"] = "Postarea a fost modificata!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    return View(requestPhoto);
                }

            }
            catch (Exception e)
            {
                return View(requestPhoto);
            }
        }

        // GET: PhotoFlickr/Delete/5     
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            PhotoFlickr photoFlickr = db.Photos.Find(id);
            if (photoFlickr.UserId == User.Identity.GetUserId() ||
                User.IsInRole("Administrator"))
            {
                db.Photos.Remove(photoFlickr);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un articol care nu va apartine!";
                return RedirectToAction("Index");
            }

        }

  
        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }

            // returnam lista de categorii
            return selectList;
        }
    }
}
