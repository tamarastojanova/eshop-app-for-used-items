using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class Basket
    {
        [Key]
        [Column("id_basket")]
        public int Id { get; set; }
        [Column("id_user")]
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}