using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class OrderContainsItem
    {
        [Key]
        [Column("id_order", Order = 0)]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [Key]
        [Column("id_item", Order = 1)]
        public int IdItem { get; set; }

        [ForeignKey("IdItem")]
        public virtual Item Item { get; set; }

        [Column("quantity")]
        [Required]
        public int Quantity { get; set; }
    }
}