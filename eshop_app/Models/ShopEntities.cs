using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class ShopEntities : DbContext
    {
        public DbSet<Product> Products { get; set;}
        public DbSet<DeliveryPerson> DeliveryPeople { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }


        public ShopEntities() : base("name=ShopEntities") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("eshop_product", "eshop_application");
            modelBuilder.Entity<DeliveryPerson>().ToTable("eshop_deliveryperson", "eshop_application");
            modelBuilder.Entity<Category>().ToTable("eshop_category", "eshop_application");
            modelBuilder.Entity<User>().ToTable("eshop_user", "eshop_application");
            modelBuilder.Entity<Administrator>().ToTable("eshop_administrator", "eshop_application");
            modelBuilder.Entity<Customer>().ToTable("eshop_customer", "eshop_application");
            modelBuilder.Entity<Seller>().ToTable("eshop_seller", "eshop_application");
            modelBuilder.Entity<Basket>().ToTable("eshop_basket", "eshop_application");
            modelBuilder.Entity<Item>().ToTable("eshop_item", "eshop_application");
            modelBuilder.Entity<Order>().ToTable("eshop_order", "eshop_application");
            modelBuilder.Entity<OtherImages>().ToTable("eshop_other_images", "eshop_application");
            modelBuilder.Entity<BasketContainsItem>().ToTable("eshop_basket_contains_item", "eshop_application");
            modelBuilder.Entity<OrderContainsItem>().ToTable("eshop_order_contains_item", "eshop_application");
            modelBuilder.Entity<ProductIsOfCategory>().ToTable("eshop_product_is_of_category", "eshop_application");
            //            modelBuilder.HasSequence<int>("tracking_number_sequence").StartsAt(1000);

        }

        internal static ShopEntities Create()
        {
            return new ShopEntities();
        }

        public System.Data.Entity.DbSet<eshop_app.Models.BasketContainsItem> BasketContainsItems { get; set; }

        public System.Data.Entity.DbSet<eshop_app.Models.OtherImages> OtherImages { get; set; }

        public System.Data.Entity.DbSet<eshop_app.Models.ProductIsOfCategory> ProductIsOfCategories { get; set; }

        public System.Data.Entity.DbSet<eshop_app.Models.OrderContainsItem> OrderContainsItems { get; set; }
    }
}