using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CuStore.Domain.Entities
{
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Name")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product code is required")]
        [Display(Name = "Code")]
        [Remote("CheckProductCodeUniquness", "Manage", areaName: "Admin")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        [Index]
        public string Code { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price is required")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Product quanity in stock is required")]
        [Display(Name = "Quanity in stock")]
        public int QuanityInStock { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; } 

        [Required(ErrorMessage = "Product category is required")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
