using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class Item
    {
        [Key]
        [Column("id_item")]

        public int Id { get; set; }

        [MaxLength(100)]
        [Column("item_condition")]
        [Required(ErrorMessage = "Please choose the item condition.")]
        public string ItemCondition { get; set; }

        [MaxLength(255)]
        [Column("main_image")]
        public string MainImage { get; set; }

        [Column("id_user")]
        public String SellerId { get; set; }
        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; }

        [Column("id_product")]
        [Required(ErrorMessage = "Please choose a product.")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Column("quantity")]
        [Required(ErrorMessage = "Please enter available quantity.")]
        public int Quantity { get; set; }

        [MaxLength(500)]
        [Column("listing_title")]
        [Required(ErrorMessage = "Please enter the title of your listing.")]
        public string ListingTitle { get; set; }

        [MaxLength(2000)]
        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        [Required(ErrorMessage = "Please define a price.")]
        public double Price { get; set; }

        [Column("date_added_for_sale")]
        public DateTime DateAddedForSale { get; set; }

        [Column("weight")]
        [Required(ErrorMessage = "Please specify the item weight.")]
        public double Weight { get; set; }

        [Column("location")]
        [Required(ErrorMessage = "Please specify the location.")]
        public string Location { get; set; }

        [Column("upforsale")]
        [Required]
        public bool upForSale { get; set; }
    }
}