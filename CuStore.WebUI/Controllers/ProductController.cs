using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Models;
using CuStore.WebUI.ViewModels;

namespace CuStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IStoreRepository _repositiry;
        private int _pageSize = 5;

        public ProductController(IStoreRepository storeRepository)
        {
            this._repositiry = storeRepository;
        }

        // GET: Product
        public ViewResult List(int? categoryId, int pageNumber = 1)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = _repositiry.GetProductsByCategory(_pageSize, pageNumber, categoryId),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = _pageSize,
                    TotalItems = _repositiry.GetProductsCount(categoryId)
                },
                CurrentCategory = _repositiry.GetCategoryById(categoryId.GetValueOrDefault())
            };

            return View(viewModel);
        }

        public ViewResult Details(int productId, string returnUrl)
        {
            var viewModel = new ProductDetailsViewModel
            {
                Product = _repositiry.GetProductById(productId),
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }
    }
}