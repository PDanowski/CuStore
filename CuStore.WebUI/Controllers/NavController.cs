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
        private IStoreRepository _repositiry;

        public NavController(IStoreRepository storeRepository)
        {
            this._repositiry = storeRepository;
        }

        public PartialViewResult Menu()
        {
            CategoriesListViewModel viewModel = new CategoriesListViewModel
            {
                Categories = _repositiry.GetCategories()
            };

            return PartialView(viewModel);
        }


    }
}