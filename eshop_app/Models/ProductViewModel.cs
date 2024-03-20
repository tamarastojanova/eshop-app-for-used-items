using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class ProductViewModel
    {
        [Required(ErrorMessage = "Please choose a category.")]
        public int IdCategory { get; set; }

        [Required(ErrorMessage = "Please enter a product name.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please enter an image.")]
        [MaxLength(255, ErrorMessage = "Product URL should not be more than 255 characters.")]
        public string ProductUrl { get; set; }
    }
}