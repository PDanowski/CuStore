using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Areas.Admin.ViewModels;
using CuStore.WebUI.Infrastructure.Helpers;

namespace CuStore.WebUI.Areas.Admin.Controllers
{
    //[RouteArea(areaName: "Admin")]
    public class ManageController : Controller
    {
        private readonly IStoreRepository _repository;

        public ManageController(IStoreRepository repository)
        {
            this._repository = repository;
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
        public ViewResult ManageProducts()
        {
            return View(_repository.GetProducts());
        }


        public ViewResult EditProduct(int productId)
        {
            Product product = _repository.GetProductById(productId);
            EditProductViewModel viewModel = new EditProductViewModel
            {
                Product = product,
                Categories = CategroriesProvider.CreateSelectList(_repository.GetCategories().ToList())
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

                bool isSaved = _repository.SaveProduct(viewModel.Product);

                if (isSaved)
                {
                    //TempData is removed at end of request
                    //used because of redirection
                    TempData["message"] = String.Format("Saved {0}", viewModel.Product.Name);
                    return RedirectToAction("ManageProducts");
                }           
            }

            TempData["message"] = "Error during saving product";
            viewModel.Categories = CategroriesProvider.CreateSelectList(_repository.GetCategories().ToList());
            return View(viewModel);
        }

        public ViewResult CreateProduct()
        {
            EditProductViewModel viewModel = new EditProductViewModel
            {
                Product = new Product(),
            Categories = CategroriesProvider.CreateSelectList(_repository.GetCategories().ToList())
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

                bool isCreated = _repository.AddProduct(viewModel.Product);

                if (isCreated)
                {
                    //TempData is removed at end of request
                    //used because of redirection
                    TempData["message"] = String.Format("Created {0}", viewModel.Product.Name);
                }
                return RedirectToAction("ManageProducts");
            }

            TempData["message"] = "Error during creating product";
            viewModel.Categories = CategroriesProvider.CreateSelectList(_repository.GetCategories().ToList());
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteProduct(int productId)
        {
                bool isDeleted = _repository.RemoveProduct(productId);

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

        [Authorize(Roles = "Admin")]
        [Route("Admin/Categories")]
        public ViewResult ManageCategories()
        {
            return View(_repository.GetCategories());
        }

        public ViewResult EditCategory(int categoryId)
        {
            Category category = _repository.GetCategoryById(categoryId);
            EditCategoryViewModel viewModel = new EditCategoryViewModel
            {
                Category = category,
                ParentCategories = CategroriesProvider.CreateSelectList(_repository.GetParentCategories().ToList())
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditCategory(EditCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                bool isSaved = _repository.SaveCategory(viewModel.Category);

                if (isSaved)
                {
                    //TempData is removed at end of request
                    //used because of redirection
                    TempData["message"] = String.Format("Saved {0}", viewModel.Category.Name);
                    return RedirectToAction("ManageCategories");
                }
            }

            TempData["message"] = "Error during saving category";
            viewModel.ParentCategories = CategroriesProvider.CreateFlatSelectList(_repository.GetParentCategories().ToList());
            return View(viewModel);
        }

        public ViewResult CreateCategory()
        {
            EditCategoryViewModel viewModel = new EditCategoryViewModel
            {
                Category = new Category(),
                ParentCategories = CategroriesProvider.CreateFlatSelectList(_repository.GetParentCategories().ToList())
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateCategory(EditCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = _repository.AddCategory(viewModel.Category);

                if (isCreated)
                {
                    //TempData is removed at end of request
                    //used because of redirection
                    TempData["message"] = String.Format("Created {0}", viewModel.Category.Name);
                }
                return RedirectToAction("ManageCategories");
            }

            TempData["message"] = "Error during creating category";
            viewModel.ParentCategories = CategroriesProvider.CreateFlatSelectList(_repository.GetParentCategories().ToList());
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteCategory(int categoryId)
        {
            bool isDeleted = _repository.RemoveCategory(categoryId);

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
    }
}