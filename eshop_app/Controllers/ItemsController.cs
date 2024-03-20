using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eshop_app.Models;
using Microsoft.AspNet.Identity;

namespace eshop_app.Controllers
{
    public class ItemsController : Controller
    {
        private ShopEntities db = new ShopEntities();

        // GET: Items
        public ActionResult Index()
        {
            List<ItemsForSaleViewModel> resultList = new List<ItemsForSaleViewModel>();
            if (TempData["ItemsList"] != null)
                resultList = (List<ItemsForSaleViewModel>)TempData["ItemsList"];
            else
            {
                List<ItemsForSale> rows;
                if (TempData["rows"] != null)
                {
                    rows = (List<ItemsForSale>)TempData["rows"];
                }
                else
                {
                    var sqlQuery = "select * from eshop_application.items_for_sale";
                    rows = db.Database.SqlQuery<ItemsForSale>(
                        sqlQuery).ToList();
                }

                Dictionary<int, List<Category>> CategoriesPerItem = new Dictionary<int, List<Category>>();
                foreach (var r in rows)
                {
                    CategoriesPerItem.TryGetValue(r.id_item, out List<Category> categories);
                    if (categories == null) categories = new List<Category>();
                    if (!categories.Contains(db.Categories.Where(c => c.Id == r.id_category).FirstOrDefault()))
                        categories.Add(db.Categories.Where(c => c.Id == r.id_category).FirstOrDefault());
                    CategoriesPerItem[r.id_item] = categories;
                    var tmp = r;

                    ItemsForSaleViewModel itemsForSaleViewModel = new ItemsForSaleViewModel
                    {
                        id_product = tmp.id_product,
                        product_name = tmp.product_name,
                        id_item = tmp.id_item,
                        listing_title = tmp.listing_title,
                        description = tmp.description,
                        date_added_for_sale = tmp.date_added_for_sale,
                        location = tmp.location,
                        main_image = tmp.main_image,
                        id_user = tmp.id_user,
                        price = tmp.price,
                        Categories = categories 
                    };
                    if (!resultList.Any(ifs => ifs.id_item == r.id_item))
                        resultList.Add(itemsForSaleViewModel);
                }

            }
            var user = db.Users.Find(User.Identity.GetUserId());
            if(user != null)
            {
                ViewBag.IdUser = user.Id;
                ViewBag.isAdministrator = (user is Administrator);
            }
            else
            {
                ViewBag.IdUser = null;
                ViewBag.isAdministrator = false;
            }
            return View(resultList);
        }

        public ActionResult MyListings()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user != null)
            {
                var sqlQuery = "select *\r\n" +
                    "from eshop_application.my_listings ml\r\n" +
                    "where ml.id_user = @Id";
                List<ItemsForSale> rows = db.Database.SqlQuery<ItemsForSale>(
                    sqlQuery,
                    new Npgsql.NpgsqlParameter("@Id", NpgsqlTypes.NpgsqlDbType.Text) { Value = user.Id }).ToList();
                TempData["rows"] = rows;
                return RedirectToAction("Index", "Items");
            }
            string returnUrl = Url.Action("Login", "Account", new { returnUrl = "/Items/MyListings" });
            return Redirect(returnUrl);

        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sqlQuery = "select * from eshop_application.details_for_item where id_item="+id.ToString();
            var rows = db.Database.SqlQuery<DetailsForItem>(sqlQuery).ToList();
            if (rows.Count == 0)
            {
                return HttpNotFound();
            }
            List<string> other_images = new List<string>();
            List<Category> categories = new List<Category>();
            foreach (var r in rows)
            {
                if (r.other_image != null && !other_images.Contains(r.other_image))
                    other_images.Add(r.other_image);

                if (r.id_category != null && !categories.Any(c => c.Id == r.id_category))
                    categories.Add(db.Categories.Where(c => c.Id == r.id_category).FirstOrDefault());

            }
            DetailsForItem tmp = rows[0];
            DetailsForItemViewModel detailsForItemViewModel = new DetailsForItemViewModel
            {
                id_item = tmp.id_item,
                other_images = other_images,
                main_image = tmp.main_image,
                categories = categories,
                id_product = tmp.id_product,
                id_user = tmp.id_user,
                listing_title = tmp.listing_title,
                description = tmp.description,
                quantity = tmp.quantity,
                item_condition = tmp.item_condition,
                price = tmp.price,
                profile_picture = tmp.profile_picture,
                seller_name = tmp.seller_name,
                surname = tmp.surname,
                email = tmp.email,
                phone_number = tmp.phone_number
            };
            var user = db.Users.Find(User.Identity.GetUserId());
            bool isAlreadyInCart = false;
            bool theirItem = false;
            if (user != null)
            {
                Basket userBasket = db.Baskets.Where(b => b.UserId.Equals(user.Id)).FirstOrDefault();
                isAlreadyInCart = db.BasketContainsItems
                    .Any(row => row.IdItem == tmp.id_item && row.BasketId == userBasket.Id);
                theirItem = (tmp.id_user == user.Id);
            }
            ViewBag.isAlreadyInCart = isAlreadyInCart;
            ViewBag.theirItem = theirItem;
            Item item = db.Items.Where(i => i.Id.Equals(tmp.id_item)).FirstOrDefault();
            ViewBag.heavy = (item.Weight >= 5);
            return View(detailsForItemViewModel);
        }

        // GET: Items1/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                string returnUrl = Url.Action("Login", "Account", new { returnUrl = "/Items/Create"});
                return Redirect(returnUrl);
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            ViewBag.SellerId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Items1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemCondition,MainImage,ProductId,Quantity,ListingTitle,Description,Price,Weight,Location")] Item item)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            item.DateAddedForSale = DateTime.Now;
            item.SellerId = user.Id;
            item.upForSale = true;
            if (item.MainImage == null)
            {
                item.MainImage = "https://newadmin.heberjeunes.fr/images/no-photo.png";
            }
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", item.ProductId);
            ViewBag.SellerId = new SelectList(db.Users, "Id", "Name", item.SellerId);
            return View(item);
        }


        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                string returnUrl = Url.Action("Login", "Account", new { returnUrl = "/Items/Edit?id=" + id.ToString() });
                return Redirect(returnUrl);
            }
            if (user.Id != item.SellerId)
            {
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", item.ProductId);
            ViewBag.SellerId = new SelectList(db.Users, "Id", "Name", item.SellerId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemCondition,MainImage,upForSale,SellerId,ProductId,Quantity,ListingTitle,Description,Price,DateAddedForSale,Weight,Location")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", item.ProductId);
            ViewBag.SellerId = new SelectList(db.Users, "Id", "Name", item.SellerId);
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if(user == null)
            {
                string returnUrl = Url.Action("Login", "Account", new { returnUrl = "/Items/Delete?id=" + id.ToString() });
                return Redirect(returnUrl);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            if(user.Id != item.SellerId && !(user is Administrator))
            {
                return RedirectToAction("Index");
            }
            item.upForSale = false;
            db.SaveChanges();
            List<BasketContainsItem> bci = db.BasketContainsItems.Where(b => b.IdItem == item.Id).ToList();
            foreach(var b in bci)
            {
                db.BasketContainsItems.Remove(b);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
