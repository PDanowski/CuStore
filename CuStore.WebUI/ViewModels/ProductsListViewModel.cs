using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CuStore.Domain.Entities;
using CuStore.WebUI.Models;

namespace CuStore.WebUI.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public Category CurrentCategory { get; set; }
    }
}