using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class AllAvailableCategoriesViewModel
    {
        public int id_category { get; set; }
        public string name { get; set; }
        public int? id_supercategory { get; set; }
    }
}