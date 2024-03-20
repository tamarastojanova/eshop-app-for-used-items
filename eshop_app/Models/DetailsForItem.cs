using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class DetailsForItem
    {
        public int id_item { get; set; }
        public string other_image { get; set; }
        public string main_image { get; set; }
        public string listing_title { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public string item_condition { get; set; }
        public double price { get; set; }
        public int id_product { get; set; }
        public int? id_category { get; set; }
        public string category_name { get; set; }
        public string id_user { get; set; }
        public string profile_picture { get; set; }
        public string seller_name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
    }
}