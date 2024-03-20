using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class ItemsInBasketHelperViewModel
    {
        public int id_basket { get; set; }
        public string id_user { get; set;}
        public string profile_picture { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public int id_item { get; set; }
        public string listing_title { get; set; }
        public string main_image { get; set; }
        public double price { get; set; }
        public double weight { get; set; }
        public int quantity { get; set; }
    }
}