using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Entities;

namespace CuStore.WebUI.ViewModels
{
    public class EditProductViewModel
    {
        public Product Product { get; set; }
        public List<SelectListItem> Categories { get; set; }

        public EditProductViewModel()
        {
            Categories = new List<SelectListItem>();
        }
    }

}