using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CuStore.Domain.Entities;

namespace CuStore.WebUI.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public string ReturnUrl { get; set; }
    }
}