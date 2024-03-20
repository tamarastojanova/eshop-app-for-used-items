using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class OtherImages
    {
        [Key]
        [Column("id_item", Order = 0)]
        public int ItemId { get; set; }

        [Key]
        [Column("other_image", Order = 1)]
        [MaxLength(255)]
        public string Image { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
    }
}