using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.ViewModels;
using Microsoft.AspNet.Identity;

namespace CuStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IStoreRepository _repository;

        public CartController(IStoreRepository repository)
        {
            this._repository = repository;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            var product = _repository.GetProductById(productId);

            if (product != null)
            {
                cart.AddProduct(product, 1);

                if (User.Identity.IsAuthenticated)
                {
                    _repository.SaveCart(cart);
                }
            }

            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            cart.RemoveProduct(productId);

            if (User.Identity.IsAuthenticated)
            {
                if (!cart.CartItems.Any())
                {
                    _repository.RemoveCart(cart);
                    return RedirectToAction("List", "Product");
                }

                _repository.SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        [Authorize]
        public ViewResult Checkout(Cart cart)
        {
            var user = _repository.GetUserById(User.Identity.GetUserId());
            cart.UserId = user.Id;
            cart.User = user;

            var viewModel = new OrderDetailsViewModel
            {
                Order = new Order(cart),
                ShippingMethods = new List<SelectListItem>(),
                OrderValue = cart.GetValue()
            };

            viewModel.SetShippingMethods(_repository.GetShippingMethods().ToList());
            return View(viewModel);
        }
    }
}