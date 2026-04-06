using System.ComponentModel.DataAnnotations;

namespace CuStore.Core.Entities;

public class User
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is mandatory")]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is mandatory")]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    public Guid? CrmGuid { get; set; }

    public int? UserAddressId { get; set; }
    public UserAddress? UserAddress { get; set; }

    public ICollection<Cart> Carts { get; set; } = new HashSet<Cart>();
}
