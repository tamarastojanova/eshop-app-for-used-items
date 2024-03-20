using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eshop_app.Models;
using Microsoft.Ajax.Utilities;

namespace eshop_app.Controllers
{
    public class CategoriesController : Controller
    {
        private ShopEntities db = new ShopEntities();


        // GET: Categories
        public ActionResult Index()
        {
            List<Category> mainCategories = db.Categories.Where(c => c.SupercategoryId == null).ToList();
            List<CategoryLevel2> resultList = new List<CategoryLevel2>();
            foreach(Category mc in mainCategories)
            {
                CategoryLevel2 mainC = new CategoryLevel2 
                { 
                    mainCategory = mc, 
                    Categories = db.Categories.Where(cat => cat.SupercategoryId == mc.Id).ToList() 
                };
                resultList.Add(mainC);
            }
            return View(resultList);
        }
        // GET: Categories/ShowProducts/5
        public ActionResult ShowProducts(int? id)
        {
           List<Product> products = db.Products.Where(p => db.ProductIsOfCategories.Any(pioc => p.Id == pioc.ProductId
           && db.Categories.Any(c => pioc.IdCategory == c.Id && (c.SupercategoryId == id || c.Id == id)))).ToList();

           TempData["ProductsList"] = products;

           return RedirectToAction("Index", "Products");
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.SupercategoryId = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryName,SupercategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupercategoryId = new SelectList(db.Categories, "Id", "CategoryName", category.SupercategoryId);
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupercategoryId = new SelectList(db.Categories, "Id", "CategoryName", category.SupercategoryId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryName,SupercategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupercategoryId = new SelectList(db.Categories, "Id", "CategoryName", category.SupercategoryId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
