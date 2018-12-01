using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CuStore.Domain.Entities
{
    public class User : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public int? AddressId { get; set; }

        public Address Address { get; set; }

        #region Additional properties
        [Required(ErrorMessage = "First name is mandatory")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is mandatory")]
        public string LastName { get; set; }

        public string Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        #endregion

        public ICollection<Cart> Carts { get; set; }
    }
}
