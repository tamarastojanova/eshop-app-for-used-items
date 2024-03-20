using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eshop_app.Models
{
    public class DeliveryPerson
    {
        [Key]
        [Column("id_deliveryperson")]
        public int Id { get; set; }
        [Column("name")]
        [MaxLength(50)]
        [Required]
        public String Name { get; set; }
        [Column("surname")]
        [MaxLength(50)]
        [Required]
        public String Surname { get; set; }
        [Column("email")]
        [MaxLength(100)]
        [Required]
        public String Email { get; set; }
        [Column("phone_number")]
        [MaxLength(20)]
        [Required]
        public String PhoneNumber { get; set; }
        [Column("profile_picture")]
        [MaxLength(255)]
        [Required]
        public String ProfilePicture { get; set; } 

    }
}