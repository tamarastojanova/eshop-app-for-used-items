using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class ProductIsOfCategory
    {
        [Key]
        [Column("id_product", Order = 0)]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Key]
        [Column("id_category", Order = 1)]
        public int IdCategory { get; set; }

        [ForeignKey("IdCategory")]
        public virtual Category Category { get; set; }
    }
}