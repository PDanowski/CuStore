using System.Collections.Generic;
using System.Web.Mvc;
using CuStore.Domain.Entities;

namespace CuStore.WebUI.Areas.Admin.ViewModels
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