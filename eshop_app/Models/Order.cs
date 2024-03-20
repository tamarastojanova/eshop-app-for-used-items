using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class Order
    {
        [Key]
        [Column("id_order")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Column("status")]
        [Required]
        public string Status { get; set; }

        [Column("date_made")]
        [Required]
        public DateTime DateMade { get; set; }

        [Column("date_delivered")]
        public DateTime? DateDelivered { get; set; }

        [MaxLength(150)]
        [Column("street")]
        [Required(ErrorMessage = "Street name field is required.")]
        public string StreetName { get; set; }

        [MaxLength(10)]
        [Column("house_number")]
        public string HouseNumber { get; set; }

        [MaxLength(100)]
        [Column("city")]
        [Required(ErrorMessage = "City field is required.")]
        public string CityName { get; set; }

        [MaxLength(10)]
        [Column("zip")]
        [Required(ErrorMessage = "Zip field is required.")]
        public string Zip { get; set; }

        [MaxLength(100)]
        [Column("country")]
        [Required(ErrorMessage = "Country field is required.")]
        public string CountryName { get; set; }

        [Column("id_user")]
        [Required]
        public String CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }

        [Column("id_basket")]
        [Required]
        public int BasketId { get; set; }
        [ForeignKey("BasketId")]
        public virtual Basket Basket { get; set; }

        [Column("id_deliveryperson")]
        public int? DeliveryPersonId { get; set; }
        [ForeignKey("DeliveryPersonId")]
        public virtual DeliveryPerson DeliveryPerson { get; set; }

        [MaxLength(20)]
        [Column("tracking_number")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string TrackingNumber { get; set; }

    }
}