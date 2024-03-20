using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class UndeliveredOrdersViewModel
    {
        public int id_order { get; set; }
        public string tracking_number { get; set; }
        public string status { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime date_made { get; set; }
        public string street { get; set; }
        public string house_number { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string id_user { get; set; }
        public string customer_profile_picture { get; set; }
        public string customer_name { get; set; }
        public string customer_surname { get; set; }
        public string customer_phone_number { get; set; }
        public int? id_deliveryperson { get; set; }
        public string deliveryperson_profile_picture { get; set; }
        public string deliveryperson_name { get; set; }
        public string deliveryperson_surname { get; set; }
        public string deliveryperson_phone_number { get; set; }
    }
}