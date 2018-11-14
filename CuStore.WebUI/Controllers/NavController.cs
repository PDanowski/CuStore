using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.WebUI.ViewModels;

namespace CuStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly IStoreRepository _repositiry;

        public NavController(IStoreRepository storeRepository)
        {
            this._repositiry = storeRepository;
        }

        public PartialViewResult Menu(int? selectedCategoryId = null)
        {
            var categories = _repositiry.GetCategories().ToList();

            CategoriesListViewModel viewModel = new CategoriesListViewModel
            {
                Categories = categories,
                SelectedCategoryId = categories.FirstOrDefault(c => c.Id.Equals(selectedCategoryId))?.Id
            };

            return PartialView(viewModel);
        }


    }
}