using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using CuStore.Domain.Abstract;
using CuStore.WebUI.ViewModels;

namespace CuStore.WebUI.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class NavController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public NavController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public PartialViewResult Menu(int? selectedCategoryId = null)
        {
            var categories = _categoryRepository.GetCategories().ToList();

            CategoriesListViewModel viewModel = new CategoriesListViewModel
            {
                Categories = categories,
                SelectedCategoryId = categories.FirstOrDefault(c => c.Id.Equals(selectedCategoryId))?.Id
            };

            return PartialView(viewModel);
        }


    }
}