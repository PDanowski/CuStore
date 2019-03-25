using System.ComponentModel.DataAnnotations;

namespace CuStore.Domain.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "City is mandatory")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is mandatory")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Street is mandatory")]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Postal code is mandatory")]
        [Display(Name = "Postal code")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

    }
}
