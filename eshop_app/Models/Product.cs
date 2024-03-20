using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class Product
    {
        [Key]
        [Column("id_product")]
        public int Id { get; set; }
        [Column("name")]
        [MaxLength(100)]
        [Required]
        [Display(Name = "Product name")]
        public string ProductName { get; set; }
        [Column("image")]
        [MaxLength(255)]
        [Required]
        [Display(Name = "Product image")]
        public string ProductImageURL { get; set; }
    }
}