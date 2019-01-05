using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Infrastructure.Helpers;
using CuStore.WebUI.ViewModels;
using Microsoft.AspNet.Identity;

namespace CuStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IStoreRepository _repository;
        private readonly IEmailSender _emailSender;

        public CartController(IStoreRepository repository, IEmailSender emailSender)
        {
            this._repository = repository;
            this._emailSender = emailSender;
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
                    //return RedirectToAction("List", "Product");
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

            var viewModel = new CheckoutViewModel
            {
                Cart = cart,
                ShippingMethods = ShippingMethodsProvider.CreateSelectList(_repository.GetShippingMethods().ToList()),
                OrderValue = cart.GetValue(),
            };

            return View(viewModel);
        }


        [Authorize]
        [HttpPost]
        public ViewResult Checkout(CheckoutViewModel viewModel, Cart cart)
        {
            if (cart.CartItems.Count == 0)
            {
                ModelState.AddModelError("", @"Your cart is empty. Please add products to order.");
            }

            //Server side verification
            if (ModelState.IsValid)
            {
                Order order = new Order(
                    cart, 
                    viewModel.UseUserAddress, 
                    viewModel.SelectedShippingMethodId,
                    !viewModel.UseUserAddress ? viewModel.ShippingAddress : null);

                order.ShippingMethod = _repository.GetShippingMethodById(order.ShippingMethodId);

                _repository.AddOrder(order);

                _emailSender.ProcessOrder(order);

                return View("Completed");
            }

            viewModel.Cart = cart;
            viewModel.ShippingMethods =
                ShippingMethodsProvider.CreateSelectList(_repository.GetShippingMethods().ToList());
            viewModel.OrderValue = cart.GetValue();

            return View(viewModel);
        }
    }
}