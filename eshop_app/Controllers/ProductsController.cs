using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eshop_app.Models;
using Microsoft.AspNet.Identity;

namespace eshop_app.Controllers
{
    public class ProductsController : Controller
    {
        private ShopEntities db = new ShopEntities();

        // GET: Products
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.isAdministrator = (user is Administrator);
            List<Product> products = new List<Product>();
            if (TempData["ProductsList"] != null)
                products = (List<Product>)TempData["ProductsList"];
            else
                products = db.Products.ToList();

            return View(products);
        }

        public ActionResult FindItems(int? id)
        {
            var sqlQuery = "select * from eshop_application.items_for_sale where id_product=" + id.ToString();
            var rows = db.Database.SqlQuery<ItemsForSale>(
                sqlQuery).ToList();
            List<ItemsForSaleViewModel> resultList = new List<ItemsForSaleViewModel>();
            Dictionary<int, List<Category>> CategoriesPerItem = new Dictionary<int, List<Category>>();
            foreach (var r in rows)
            {
                CategoriesPerItem.TryGetValue(r.id_item, out List<Category> categories);
                if (categories == null) categories = new List<Category>();
                if (!categories.Contains(db.Categories.Where(c => c.Id == r.id_category).FirstOrDefault()))
                    categories.Add(db.Categories.Where(c => c.Id == r.id_category).FirstOrDefault());
                CategoriesPerItem[r.id_item] = categories;

                if(!resultList.Any(ifs => ifs.id_item == r.id_item))
                {
                    ItemsForSaleViewModel itemsForSaleViewModel = new ItemsForSaleViewModel
                    {
                        id_product = r.id_product,
                        product_name = r.product_name,
                        id_item = r.id_item,
                        listing_title = r.listing_title,
                        description = r.description,
                        date_added_for_sale = r.date_added_for_sale,
                        location = r.location,
                        main_image = r.main_image,
                        price = r.price,
                        Categories = categories
                    };
                    resultList.Add(itemsForSaleViewModel);
                }
            }
         
            TempData["ItemsList"] = resultList;

            return RedirectToAction("Index", "Items");
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if(user == null || !(user is Administrator))
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null || !(user is Administrator))
            {
                return RedirectToAction("Index");
            }
            ViewBag.IdCategory = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel model)
        {
            // Check if "Select category" is chosen
            if (model.IdCategory == 0)
            {
                ModelState.AddModelError("IdCategory", "Please select a category.");
            }

            if (model.ProductUrl?.Length > 255)
            {
                ModelState.AddModelError("ProductUrl", "Product URL should not be more than 255 characters.");
            }

            if (ModelState.IsValid)
            {
                Product p = new Product { ProductName = model.ProductName, ProductImageURL = model.ProductUrl };
                db.Products.Add(p);
                db.SaveChanges();
                db.ProductIsOfCategories.Add(new ProductIsOfCategory { IdCategory = model.IdCategory, ProductId = p.Id });
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.IdCategory = new SelectList(db.Categories, "Id", "CategoryName", model.IdCategory);

            return View(model);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null || !(user is Administrator))
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductName,ProductImageURL")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null || !(user is Administrator))
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
