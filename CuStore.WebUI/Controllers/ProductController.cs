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
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private int _pageSize = 5;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this._productRepository = productRepository;
            this._categoryRepository = categoryRepository;
        }

        // GET: Product
        public ViewResult List(int? categoryId, int pageNumber = 1)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = _productRepository.GetProductsByCategory(_pageSize, pageNumber, categoryId),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = _pageSize,
                    TotalItems = _productRepository.GetProductsCount(categoryId)
                },
                CurrentCategory = _categoryRepository.GetCategoryById(categoryId.GetValueOrDefault())
            };

            return View(viewModel);
        }

        // GET: Product
        public ViewResult SearchingResultsList(string phrase, int? categoryId, int pageNumber = 1)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = _productRepository.GetProductsByPhrase(phrase, _pageSize, pageNumber, categoryId),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = _pageSize,
                    TotalItems = _productRepository.GetProductsCountByPhrase(phrase, categoryId)
                },
                CurrentCategory = _categoryRepository.GetCategoryById(categoryId.GetValueOrDefault())
            };

            ViewData["isSearchingResult"] = true;
            ViewData["searchingPhrase"] = phrase;

            return View("List", viewModel);
        }

        public FileContentResult GetImage(int productId)
        {
            Product product = _productRepository.GetProductById(productId);

            return product == null ? null : File(product.ImageData, product.ImageMimeType);
        }

        public ViewResult Details(int productId, string returnUrl)
        {
            var viewModel = new ProductDetailsViewModel
            {
                Product = _productRepository.GetProductById(productId),
                ReturnUrl = returnUrl,
                Image = GetImage(productId)
            };

            return View(viewModel);
        }
    }
}