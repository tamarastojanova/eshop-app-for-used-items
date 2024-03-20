using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class FullBasketViewModel
    {
        public int id_basket { get; set; }
        public string id_user { get; set;}
        public string profile_picture { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public List<Item> items { get; set; }
        public double subtotal { get; set; }
        public double shipping_cost { get; set; }
        public double total_cost { get; set; }
        public Dictionary<int, int> quantityAddedForItemWithId { get; set; }
    }
}