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
    public class BasketsController : Controller
    {
        private ShopEntities db = new ShopEntities();

        // GET: Baskets
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                string returnUrl = Url.Action("Login", "Account", new { returnUrl = "/Baskets" });
                return Redirect(returnUrl);
            }
            var sqlQuery = "select * from eshop_application.full_basket_view where id_user=@Id";
            var rows = db.Database.SqlQuery<FullBasket>(
                sqlQuery,
                new Npgsql.NpgsqlParameter("@Id", NpgsqlTypes.NpgsqlDbType.Text) { Value = user.Id }).ToList();
            List<Item> items = new List<Item>();
            Dictionary<int, int> quantityAddedForItemWithId = new Dictionary<int, int>();
            foreach (var r in rows)
            {
                items.Add(db.Items.Where(i => i.Id == r.id_item).FirstOrDefault());
                quantityAddedForItemWithId[r.id_item] = r.quantity_item;
            }
            
            FullBasketViewModel fullBasketViewModel;
            if (rows.Count > 0)
            {
                FullBasket tmp = rows[0];
                fullBasketViewModel = new FullBasketViewModel
                {
                    id_basket = tmp.id_basket,
                    id_user = tmp.id_user,
                    profile_picture = tmp.profile_picture,
                    name = tmp.name,
                    surname = tmp.surname,
                    items = items,
                    subtotal = tmp.subtotal,
                    shipping_cost = tmp.shipping_cost,
                    total_cost = tmp.total_cost,
                    quantityAddedForItemWithId = quantityAddedForItemWithId
                };
            }
            else
            {
                fullBasketViewModel = new FullBasketViewModel
                {
                    id_basket = 0,
                    id_user = null,
                    profile_picture = null,
                    name = null,    
                    surname = null,
                    items = new List<Item>(),
                    subtotal = 0,
                    shipping_cost = 0,
                    total_cost = 0,
                    quantityAddedForItemWithId = quantityAddedForItemWithId
                };
            }
            return View(fullBasketViewModel);
        }

        [HttpPost]
        public ActionResult AddItem()
        {
            int quantity = Convert.ToInt32(Request.Form["quantity"]);
            int itemId = Convert.ToInt32(Request.Form["itemId"]);
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                string returnUrl = Url.Action("Login", "Account", new { returnUrl = "/Items/Details/" + itemId.ToString() });
                return Redirect(returnUrl);
            }
            Basket userBasket = db.Baskets.Where(b => b.UserId.Equals(user.Id)).FirstOrDefault();
            BasketContainsItem newItem = new BasketContainsItem
            {
                BasketId = userBasket.Id,
                IdItem = itemId,
                Quantity = quantity
            };
            db.BasketContainsItems.Add(newItem);
            db.SaveChanges();
            return RedirectToAction("Index", "Baskets"); // Redirect to the Basket or Cart page
        }

        // /Baskets/RemoveItem/5
        public ActionResult RemoveItem(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            Basket userBasket = db.Baskets.Where(b => b.UserId.Equals(user.Id)).FirstOrDefault();
            BasketContainsItem itemToRemove = db.BasketContainsItems
                .Where(item => item.BasketId == userBasket.Id && item.IdItem == id).FirstOrDefault();
            db.BasketContainsItems.Remove(itemToRemove);
            db.SaveChanges();
            return RedirectToAction("Index", "Baskets");
        }

        [HttpPost]
        public ActionResult CalculatePrices(List<int> selectedItems)
        {
            decimal subtotal = 0;
            decimal shippingCost = 0;
            decimal totalCost = 0;

            if (selectedItems != null)
            {
                // Retrieve the selected items from the database
                var items = db.Items.Where(i => selectedItems.Contains(i.Id)).ToList();

                // Calculate subtotal, shipping cost, and total cost based on your logic
                subtotal = (decimal)items.Sum(item => item.Price * item.Quantity);
                shippingCost = (items.Sum(item => item.Weight) >= 1) ? 20 :
                                        (items.Sum(item => item.Weight) >= 0.5) ? 10 : 5;
                totalCost = subtotal + shippingCost;
            }

            // Return the calculated values as JSON
            return Json(new
            {
                Subtotal = subtotal,
                ShippingCost = shippingCost,
                TotalCost = totalCost
            });
        }

        // GET: Baskets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket basket = db.Baskets.Find(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            return View(basket);
        }

        // GET: Baskets/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Baskets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId")] Basket basket)
        {
            if (ModelState.IsValid)
            {
                db.Baskets.Add(basket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", basket.UserId);
            return View(basket);
        }

        // GET: Baskets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket basket = db.Baskets.Find(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", basket.UserId);
            return View(basket);
        }

        // POST: Baskets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId")] Basket basket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", basket.UserId);
            return View(basket);
        }

        // GET: Baskets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket basket = db.Baskets.Find(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            return View(basket);
        }

        // POST: Baskets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Basket basket = db.Baskets.Find(id);
            db.Baskets.Remove(basket);
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
