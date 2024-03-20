using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eshop_app.Models;
using Microsoft.AspNet.Identity;

namespace eshop_app.Controllers
{
    public class OrdersController : Controller
    {
        private ShopEntities db = new ShopEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            List<Order> orders;
            if (user != null && user is Administrator)
            {
                orders = db.Orders.ToList();
            }
            else if(user != null)
            {
                orders = db.Orders.Where(o => o.CustomerId.Equals(user.Id)).ToList();
            }
            else
            {
                string returnUrl = Url.Action("Login", "Account", new { returnUrl = "/Orders" });
                return Redirect(returnUrl);
            }
            ViewBag.isAdministrator = (user is Administrator);
            orders = orders.OrderByDescending(o => o.TrackingNumber).ToList();
            return View(orders);
        }
        public ActionResult FillOut(string itemIds)
        {
            ViewBag.SelectedItemsId = itemIds;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(string StreetName, string HouseNumber, string CityName, string Zip, string CountryName, string itemIds)
        {
            string[] selectedItemsId = itemIds.Split(',');
            var selectedIds = selectedItemsId.Select(int.Parse);
            List<Item> selectedItemsList = db.Items.Where(i => selectedIds.Contains(i.Id)).ToList();
            var user = db.Users.Find(User.Identity.GetUserId());
            Basket userBasket = db.Baskets.Where(b => b.UserId.Equals(user.Id)).FirstOrDefault();
            DateTime dateTime = DateTime.Now;
            string insertQuery = "INSERT INTO eshop_application.eshop_order(status, street, house_number, city, " +
                     "zip, country, id_user, id_basket, id_deliveryperson, date_made, date_delivered)\r\n" +
                     "VALUES ('Processing', @StreetName, @HouseNumber, @CityName, @Zip, @CountryName, @UserId, @BasketId, NULL, @dateTime, NULL)\r\n" +
                     "RETURNING id_order";

            int orderId = db.Database.SqlQuery<int>(insertQuery,
                new Npgsql.NpgsqlParameter("@StreetName", StreetName),
                new Npgsql.NpgsqlParameter("@HouseNumber", HouseNumber),
                new Npgsql.NpgsqlParameter("@CityName", CityName),
                new Npgsql.NpgsqlParameter("@Zip", Zip),
                new Npgsql.NpgsqlParameter("@CountryName", CountryName),
                new Npgsql.NpgsqlParameter("@UserId", user.Id),
                new Npgsql.NpgsqlParameter("@dateTime", dateTime),
                new Npgsql.NpgsqlParameter("@BasketId", userBasket.Id)).Single();


            Order o = db.Orders.Where(ord => ord.Id == orderId).FirstOrDefault();

            foreach (var selectedItem in selectedItemsList)
            {
                var basketItem = db.BasketContainsItems
                    .Where(b => b.BasketId == userBasket.Id && b.IdItem == selectedItem.Id)
                    .FirstOrDefault();

                OrderContainsItem oci = new OrderContainsItem
                {
                    IdItem = selectedItem.Id,
                    OrderId = o.Id,
                    Quantity = basketItem.Quantity,
                };
                db.OrderContainsItems.Add(oci);
                selectedItem.Quantity -= basketItem.Quantity;
                db.Entry(selectedItem).State = EntityState.Modified;
                
                db.BasketContainsItems.Remove(basketItem);
            }
                db.SaveChanges();
                return View(o);
        }

        public ActionResult ShowUndelivered()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.isAdministrator = (user is Administrator);
            if (user != null && user is Administrator)
            {
                var sqlQuery = "select * from eshop_application.undelivered_orders";
                List<UndeliveredOrdersViewModel> orders = db.Database.SqlQuery<UndeliveredOrdersViewModel>(
                    sqlQuery).ToList();
                orders = orders.OrderByDescending(o => o.tracking_number).ToList();
                return View(orders);
            }
            else if(user != null && !(user is Administrator))
            {
                var sqlQuery = "select *\r\nfrom eshop_application.undelivered_orders uo \r\nwhere uo.id_user = @Id";
                List<UndeliveredOrdersViewModel> orders = db.Database.SqlQuery<UndeliveredOrdersViewModel>(
                    sqlQuery,
                    new Npgsql.NpgsqlParameter("@Id", NpgsqlTypes.NpgsqlDbType.Text) { Value = user.Id }).ToList();
                orders = orders.OrderByDescending(o => o.tracking_number).ToList();
                return View(orders);
            }
            string returnUrl = Url.Action("Login", "Account", new { returnUrl = "/Orders/ShowUndelivered" });
            return Redirect(returnUrl);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            if(user == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.CanAssign = (user is Administrator);
            ViewBag.DeliveryPeople = db.DeliveryPeople.ToList();
            List<OrderContainsItem> oci = db.OrderContainsItems.Where(o => o.OrderId == id).ToList();
            List<Item> items = new List<Item>();
            List<int> quantities = new List<int>();
            Dictionary<Item, int> dict = new Dictionary<Item, int>();
            foreach (var item in oci)
            {
                dict.Add(db.Items.Find(item.IdItem), item.Quantity);
                /*items.Add(db.Items.Find(item.IdItem));
                quantities.Add(item.Quantity);*/
            }
            ViewBag.Items = dict;
           /* ViewBag.Quantities = quantities;*/
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.BasketId = new SelectList(db.Baskets, "Id", "UserId");
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "Name");
            ViewBag.DeliveryPersonId = new SelectList(db.DeliveryPeople, "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Status,DateMade,DateDelivered,StreetName,HouseNumber,CityName,Zip,CountryName,CustomerId,BasketId,DeliveryPersonId,TrackingNumber")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BasketId = new SelectList(db.Baskets, "Id", "UserId", order.BasketId);
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "Name", order.CustomerId);
            ViewBag.DeliveryPersonId = new SelectList(db.DeliveryPeople, "Id", "Name", order.DeliveryPersonId);
            return View(order);
        }
        [HttpPost]
        public ActionResult Assign(int? orderId, int? deliveryPersonId)
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == orderId);
            order.DeliveryPersonId = deliveryPersonId;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = orderId });
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.BasketId = new SelectList(db.Baskets, "Id", "UserId", order.BasketId);
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "Name", order.CustomerId);
            ViewBag.DeliveryPersonId = new SelectList(db.DeliveryPeople, "Id", "Name", order.DeliveryPersonId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Status,DateMade,DateDelivered,StreetName,HouseNumber,CityName,Zip,CountryName,CustomerId,BasketId,DeliveryPersonId,TrackingNumber")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BasketId = new SelectList(db.Baskets, "Id", "UserId", order.BasketId);
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "Name", order.CustomerId);
            ViewBag.DeliveryPersonId = new SelectList(db.DeliveryPeople, "Id", "Name", order.DeliveryPersonId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
