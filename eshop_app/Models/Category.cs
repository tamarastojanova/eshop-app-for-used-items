using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class Category
    {
        [Key]
        [Column("id_category")]
        public int Id { get; set; }
        [Column("name")]
        [Required]
        [Display(Name = "Category name")]
        public String CategoryName { get; set; }
        [Column("id_supercategory")]
        public int? SupercategoryId { get; set; }
        [ForeignKey("SupercategoryId")]
        public virtual Category Supercategory { get; set; }
    }
}