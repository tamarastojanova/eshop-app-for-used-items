using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class ItemsForSaleViewModel
    {
        public int id_product { get; set; }
        public string product_name { get; set; }
        public int id_item { get; set; }
        public string listing_title { get; set; }
        public string description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime date_added_for_sale { get; set; }
        public string location { get; set; }
        public string main_image { get; set; }
        public string id_user { get; set; }
        public double price { get; set; }
        public List<Category> Categories { get; set; } 
    }   
}