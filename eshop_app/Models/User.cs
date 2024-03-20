using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace eshop_app.Models
{
    public class User:IUser<string>
    {
        [Key]
        [MaxLength(200)]
        [Column("id_user")]
        public string Id { get; set; }
        [MaxLength(50)]
        [Column("name")]
        [Required]
        public string Name { get; set; }
        [MaxLength(50)]
        [Column("surname")]
        [Required]
        public string Surname { get; set; }
        [MaxLength(100)]
        [Column("email")]
        [Required]
        public string Email { get; set; }
        [MaxLength(30)]
        [Column("username")]
        [Required]
        public string UserName { get; set; }
        [MaxLength(200)]
        [Column("password")]
        [Required]
        public string PasswordHash { get; set; }
        [MaxLength(20)]
        [Column("phone_number")]
        [Required]
        public string PhoneNumber { get; set; }
        [MaxLength(255)]
        [Column("profile_picture")]
        [Required]
        public string ProfilePicture { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note: This example assumes that you have a UserManager with the name ApplicationUserManager

            // Create a claims identity
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom claims based on your user properties
            //userIdentity.AddClaim(new Claim("FullName", $"{FirstName} {LastName}"));
            userIdentity.AddClaim(new Claim("Email", Email));

            // Add additional claims as needed...

            return userIdentity;
        }
    }
}