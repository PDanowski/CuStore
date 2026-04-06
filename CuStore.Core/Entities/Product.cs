using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuStore.Core.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; } = string.Empty;

    [DataType(DataType.MultilineText)]
    [Display(Name = "Name")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product code is required")]
    [Display(Name = "Code")]
    [Column(TypeName = "varchar(200)")]
    [StringLength(200)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Product price is required")]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Product quanity in stock is required")]
    [Display(Name = "Quanity in stock")]
    public int QuanityInStock { get; set; }

    public byte[]? ImageData { get; set; }
    public string? ImageMimeType { get; set; }

    [Required(ErrorMessage = "Product category is required")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
}
