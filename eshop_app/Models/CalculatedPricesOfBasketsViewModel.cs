using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class CalculatedPricesOfBasketsViewModel
    {
        public int id_basket { get; set; }
        public decimal subtotal { get; set; }
        public decimal shipping_cost { get; set; }
        public decimal total_cost { get; set; }
    }
}