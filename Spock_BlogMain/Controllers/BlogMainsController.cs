using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Spock_BlogMain.Models;
using Spock_BlogMain.Utilities;
using PagedList;
using PagedList.Mvc;

namespace Spock_BlogMain.Controllers
{
    [Authorize(Roles ="Admin")]
    [RequireHttps]
    public class BlogMainsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
         

        //public ActionResult Index()
        //{
        //    return View(db.BlogMains.ToList());
        //}

        //Get:index text search,Srijana
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;
            var blogList = IndexSearch(searchStr);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
           // var listPost = db.BlogMains.AsQueryable();
            return View(blogList.ToPagedList(pageNumber, pageSize));

        }
        public IQueryable<BlogMain>IndexSearch(string searchStr)
            {
                IQueryable<BlogMain> result = null;
                if (searchStr != null)
                {

                   result = db.BlogMains.AsQueryable();
                    result = result.Where(p => p.Title.Contains(searchStr) ||
                                          p.Body.Contains(searchStr) ||
                                          p.Comments.Any(c => c.Body.Contains(searchStr) ||
                                                        c.Author.FirstName.Contains(searchStr) || 
                                                        c.Author.LastName.Contains(searchStr) || 
                                                        c.Author.DisplayName.Contains(searchStr) || 
                                                        c.Author.Email.Contains(searchStr)));

            }
                else
                {
                    result = db.BlogMains.AsQueryable();
                }
                return result.OrderByDescending(p => p.Created);
            }
      
        // GET: BlogMains/Details/ 
        [AllowAnonymous]
        public ActionResult Details(string Slug)
        {
            if (Slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var blogMain = db.BlogMains.FirstOrDefault(b => b.Slug == Slug);
            if (blogMain == null)
            {
                return HttpNotFound();
            }
            return View(blogMain);
        }

        // GET: BlogMains/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Abstract,MediaUrl,Body,Published")] BlogMain blogMain, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {   
                var Slug = StringUtilities.SlugMaker(blogMain.Title);
                if (string.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogMain);
                }   
                if (db.BlogMains.Any(p => p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogMain);
                }
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {

                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Upload/"), fileName));
                    blogMain.MediaUrl = "~/Upload/" + fileName;
                }

                blogMain.Slug = Slug;
                blogMain.Created = DateTimeOffset.Now;
                db.BlogMains.Add(blogMain);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogMain);
        }

       
        
        // GET: BlogMains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogMain blogMain = db.BlogMains.Find(id);
            if (blogMain == null)
            {
                return HttpNotFound();
            }
            return View(blogMain);
        }
        
        // POST: BlogMains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Updated,Title,Abstract,Body,Slug,MediaUrl,Published")] BlogMain blogMain, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var newSlug = StringUtilities.SlugMaker(blogMain.Title);

                if (newSlug != blogMain.Slug)
                {

                    if (string.IsNullOrWhiteSpace(newSlug))
                    {
                        ModelState.AddModelError("Title", "Invalid title");
                        return View(blogMain);
                    }
                    if (db.BlogMains.Any(p => p.Slug == newSlug))
                    {
                        ModelState.AddModelError("Title", "The title must be unique");
                        return View(blogMain);
                    }

                    blogMain.Slug = newSlug;
                }
                if (image != null)
                {
                    if (ImageUploadValidator.IsWebFriendlyImage(image))
                    {

                        string fileName = Path.GetFileName(image.FileName);
                        image.SaveAs(Path.Combine(Server.MapPath("~/Upload/"), fileName));
                        blogMain.MediaUrl = "~/Upload/" + fileName;
                    }

                    
                   
                }
                blogMain.Updated = DateTimeOffset.Now;
                db.BlogMains.Add(blogMain);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogMain);
        }
  
    
        // GET: BlogMains/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogMain blogMain = db.BlogMains.Find(id);
            if (blogMain == null)
            {
                return HttpNotFound();
            }
            return View(blogMain);
        }

        // POST: BlogMains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogMain blogMain = db.BlogMains.Find(id);
            db.BlogMains.Remove(blogMain);
            db.SaveChanges();
            return RedirectToAction("Index");
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