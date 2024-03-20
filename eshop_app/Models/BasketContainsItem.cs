using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class BasketContainsItem
    {
        [Key]
        [Column("id_basket", Order = 0)]
        public int BasketId { get; set; }

        [ForeignKey("BasketId")]
        public virtual Basket Basket { get; set; }

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