using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.Domain.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "First name is mandatory")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is mandatory")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "City is mandatory")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is mandatory")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Street is mandatory")]
        public string Street { get; set; }

        [Required(ErrorMessage = "PostalCode is mandatory")]
        public string PostalCode { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
