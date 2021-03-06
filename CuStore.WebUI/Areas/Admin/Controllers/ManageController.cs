﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Areas.Admin.ViewModels;
using CuStore.WebUI.Infrastructure.Abstract;
using CuStore.WebUI.Infrastructure.Helpers;
using CuStore.WebUI.Infrastructure.Implementations;
using CuStore.WebUI.Models;
using Microsoft.AspNet.Identity.Owin;

namespace CuStore.WebUI.Areas.Admin.Controllers
{
    //[RouteArea(areaName: "Admin")]
    public class ManageController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        private readonly ICrmClientAdapter _crmClientAdapter;

        private int _pageSize = 10;

        public ManageController(IProductRepository productRepository, 
            ICategoryRepository categoryRepository, 
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            ICrmClientAdapter crmClientAdapter)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;

            _crmClientAdapter = crmClientAdapter;
        }

        [Authorize(Roles = "Admin")]
        //[Route("~/Admin")]
        //[Route("Manage")]
        public ViewResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        //[Route("Admin/Products")]
        public ViewResult ManageProducts(int pageNumber = 1)
        {
            return View(new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = _pageSize,
                TotalItems = _productRepository.GetProductsCount()
            });
        }

        [Authorize(Roles = "Admin")]
        public ViewResult ManageOrders(int pageNumber = 1)
        {
            return View(new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = _pageSize,
                TotalItems = _orderRepository.GetOrdersCount()
            });
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult GetOrders(int pageNumber = 1)
        {
            return PartialView(_orderRepository.GetOrders(_pageSize, pageNumber));
        }

        [Authorize(Roles = "Admin")]
        [ChildActionOnly]
        public PartialViewResult GetProducts(int pageNumber = 1)
        {
            return PartialView(_productRepository.GetProductsByCategory(_pageSize, pageNumber));
        }


        [HandleError(ExceptionType = typeof(Exception), View = "ErrorDetailed")]
        public ViewResult EditProduct(int productId)
        {
            Product product = _productRepository.GetProductById(productId);
            EditProductViewModel viewModel = new EditProductViewModel
            {
                Product = product,
                Categories = CategroriesProvider.CreateSelectList(_categoryRepository.GetCategories().ToList())
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditProduct(EditProductViewModel viewModel, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    viewModel.Product.ImageMimeType = image.ContentType;
                    viewModel.Product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(viewModel.Product.ImageData, 0, image.ContentLength);
                }

                bool isSaved = _productRepository.SaveProduct(viewModel.Product);

                if (isSaved)
                {
                    //TempData is removed at end of request
                    //used because of redirection
                    TempData["message"] = $"Saved {viewModel.Product.Name}";
                    return RedirectToAction("ManageProducts");
                }           
            }

            TempData["message"] = "Error during saving product";
            viewModel.Categories = CategroriesProvider.CreateSelectList(_categoryRepository.GetCategories().ToList());
            return View(viewModel);
        }

        [HandleError(ExceptionType = typeof(Exception), View = "ErrorDetailed")]
        public ViewResult CreateProduct()
        {
            EditProductViewModel viewModel = new EditProductViewModel
            {
                Product = new Product(),
                Categories = CategroriesProvider.CreateSelectList(_categoryRepository.GetCategories().ToList())
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateProduct(EditProductViewModel viewModel, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    viewModel.Product.ImageMimeType = image.ContentType;
                    viewModel.Product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(viewModel.Product.ImageData, 0, image.ContentLength);
                }

                bool isCreated = _productRepository.AddProduct(viewModel.Product);

                if (isCreated)
                {
                    //TempData is removed at end of request
                    //used because of redirection
                    TempData["message"] = String.Format("Created {0}", viewModel.Product.Name);
                }
                return RedirectToAction("ManageProducts");
            }

            TempData["message"] = "Error during creating product";
            viewModel.Categories = CategroriesProvider.CreateSelectList(_categoryRepository.GetCategories().ToList());
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteProduct(int productId)
        {
             bool isDeleted = _productRepository.RemoveProduct(productId);

                if (isDeleted)
                {
                    TempData["message"] = "Deleted";
                }
                else
                {
                    TempData["message"] = "Error during deletion";
                }
                return RedirectToAction("ManageProducts");
        }


        [HandleError(ExceptionType = typeof(Exception), View = "ErrorDetailed")]
        public ViewResult EditOrder(int orderId)
        {
            Order order = _orderRepository.GetOrderById(orderId);
            if (order != null)
            {
                ViewData["totalValue"] = order.GetTotalValue();
            }
            else
            {
                TempData["message"] = "Error during saving order";
            }
            return View(order);
        }

        [HttpPost]
        public ActionResult EditOrder(Order order, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                bool isSaved = _orderRepository.SaveOrder(order);

                if (isSaved)
                {
                    //TempData is removed at end of request
                    //used because of redirection
                    TempData["message"] = $"Saved {order.Id}";
                    return RedirectToAction("ManageOrders");
                }
            }

            ViewData["totalValue"] = order.GetTotalValue();
            TempData["message"] = "Error during saving order";
            return View(order);
        }

        [HttpPost]
        public ActionResult DeleteOrder(int orderId)
        {
            bool isDeleted = _orderRepository.RemoveOrder(orderId);

            if (isDeleted)
            {
                TempData["message"] = "Deleted";
            }
            else
            {
                TempData["message"] = "Error during deletion";
            }
            return RedirectToAction("ManageOrders");
        }

        [Authorize(Roles = "Admin")]
        [Route("Admin/Categories")]
        public ViewResult ManageCategories()
        {
            return View(_categoryRepository.GetCategories());
        }

        [HandleError(ExceptionType = typeof(Exception), View = "ErrorDetailed")]
        public ViewResult EditCategory(int categoryId)
        {
            Category category = _categoryRepository.GetCategoryById(categoryId);
            EditCategoryViewModel viewModel = new EditCategoryViewModel
            {
                Category = category,
                ParentCategories = CategroriesProvider.CreateSelectList(_categoryRepository.GetParentCategories().ToList())
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditCategory(EditCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                bool isSaved = _categoryRepository.SaveCategory(viewModel.Category);

                if (isSaved)
                {
                    //TempData is removed at end of request
                    //used because of redirection
                    TempData["message"] = String.Format("Saved {0}", viewModel.Category.Name);
                    return RedirectToAction("ManageCategories");
                }
            }

            TempData["message"] = "Error during saving category";
            viewModel.ParentCategories = CategroriesProvider.CreateFlatSelectList(_categoryRepository.GetParentCategories().ToList());
            return View(viewModel);
        }

        [HandleError(ExceptionType = typeof(Exception), View = "ErrorDetailed")]
        public ViewResult CreateCategory()
        {
            EditCategoryViewModel viewModel = new EditCategoryViewModel
            {
                Category = new Category(),
                ParentCategories = CategroriesProvider.CreateFlatSelectList(_categoryRepository.GetParentCategories().ToList())
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateCategory(EditCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = _categoryRepository.AddCategory(viewModel.Category);

                if (isCreated)
                {
                    //TempData is removed at end of request
                    //used because of redirection
                    TempData["message"] = String.Format("Created {0}", viewModel.Category.Name);
                }
                return RedirectToAction("ManageCategories");
            }

            TempData["message"] = "Error during creating category";
            viewModel.ParentCategories = CategroriesProvider.CreateFlatSelectList(_categoryRepository.GetParentCategories().ToList());
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteCategory(int categoryId)
        {
            bool isDeleted = _categoryRepository.RemoveCategory(categoryId);

            if (isDeleted)
            {
                TempData["message"] = "Deleted";
            }
            else
            {
                TempData["message"] = "Error during deletion";
            }
            return RedirectToAction("ManageCategories");
        }

        public JsonResult CheckProductCodeUniquness(string code)
        {
            bool isUnique = _productRepository.IsProductCodeUnique(code);

            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public ViewResult ManageUsers(int pageNumber = 1, string message = null)
        {
            TempData["message"] = message;
            return View(new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = _pageSize,
                TotalItems = _userRepository.GetUsersCount()
            });
        }

        [Authorize(Roles = "Admin")]
        [ChildActionOnly]
        public PartialViewResult GetUsers(int pageNumber = 1)
        {
            return PartialView(_userRepository.GetUsers(_pageSize, pageNumber));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateCrmData()
        {
            var usersWithoutCrm = _userRepository.GetUsersWithoutCrm().ToList();
            string updateMessage = "";

            if (usersWithoutCrm.Any())
            {
                var isSuccess = _crmClientAdapter.CreateDataForCustomers(usersWithoutCrm);

                if (isSuccess)
                {
                    _userRepository.UpdateUsers(usersWithoutCrm);
                    updateMessage = "Lacking CRM data created";
                }
                else
                {
                    updateMessage = "Error during updating CRM data";
                }
            }
            else
            {
                updateMessage = "All users already have CRM data";
            }

            return RedirectToAction("ManageUsers", new { message = updateMessage });
        }
    }
}