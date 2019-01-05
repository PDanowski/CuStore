using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Entities;

namespace CuStore.WebUI.ViewModels
{
    public class EditCategoryViewModel
    {
        public Category Category { get; set; }
        public List<SelectListItem> ParentCategories { get; set; }

        public EditCategoryViewModel()
        {
            ParentCategories = new List<SelectListItem>();
        }
    }
}