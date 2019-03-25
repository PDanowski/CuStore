using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.Domain.Entities
{
    public class ShippingAddress : Address
    {

        [Required(ErrorMessage = "First name is mandatory")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is mandatory")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Last name is mandatory")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
