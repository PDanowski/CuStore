using CuStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuStore.WebUI.ViewModels
{
    public class CategoriesListViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public int? SelectedCategoryId { get; set; }
    }
}