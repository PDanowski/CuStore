using System;
using System.Linq;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Infrastructure.Abstract;
using CuStore.WebUI.Infrastructure.Helpers;
using CuStore.WebUI.ViewModels;
using Microsoft.AspNet.Identity;

namespace CuStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IShippingMethodRepository _shippingMethodRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly ICountriesProvider _countriesProvider;
        private readonly ICrmClient _crmClient;

        public CartController(IProductRepository productRepository, 
            ICartRepository cartRepository,
            IShippingMethodRepository shippingMethodRepository, 
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IEmailSender emailSender,
            ICountriesProvider countriesProvider,
            ICrmClient crmClient)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _shippingMethodRepository = shippingMethodRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _countriesProvider = countriesProvider;
            _crmClient = crmClient;
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
            var product = _productRepository.GetProductById(productId);

            if (product != null)
            {
                cart.AddProduct(product, 1);

                if (User.Identity.IsAuthenticated)
                {
                    _cartRepository.SaveCart(cart);
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
                    _cartRepository.RemoveCart(cart);
                    //return RedirectToAction("List", "Product");
                }

                _cartRepository.SaveCart(cart);
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
            var user = _userRepository.GetUserById(User.Identity.GetUserId());
            cart.UserId = user.Id;
            cart.User = user;

            var viewModel = new CheckoutViewModel
            {
                Cart = cart,
                ShippingMethods = ShippingMethodsProvider.CreateSelectList(_shippingMethodRepository.GetShippingMethods().ToList()),
                OrderValue = cart.GetValue(),
                SelectedShippingMethodId = -1
            };

            ViewBag.CountryList = _countriesProvider.FillCountryList();

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

                order.ShippingMethod = _shippingMethodRepository.GetShippingMethodById(order.ShippingMethodId);

                if (_orderRepository.AddOrder(order))
                {
                    var crmGuid = _userRepository.GetUserById(cart.UserId).CrmGuid;

                    if (crmGuid.HasValue && crmGuid.Value != Guid.Empty)
                    {
                        _crmClient.AddPointsForCustomer(crmGuid.Value, (int)order.GetTotalValue());
                    }
  
                    _emailSender.ProcessOrder(order);
                    return View("Completed");
                }
            }

            viewModel.Cart = cart;
            viewModel.ShippingMethods =
                ShippingMethodsProvider.CreateSelectList(_shippingMethodRepository.GetShippingMethods().ToList());
            viewModel.OrderValue = cart.GetValue();
            viewModel.SelectedShippingMethodId = -1;

            ViewBag.CountryList = _countriesProvider.FillCountryList();

            return View(viewModel);
        }

        public PartialViewResult TotalValue(decimal value, int? shippingMethodId)
        {
            decimal totalValue = value;

            if (shippingMethodId.HasValue && shippingMethodId != 0)
            {
                var method = _shippingMethodRepository.GetShippingMethodById(shippingMethodId.Value);
                if (method != null)
                {
                    totalValue += method.Price;
                }
            }

            return PartialView(totalValue);
        }

        [HttpPost] 
        public JsonResult GetTotalValue(decimal value, int? shippingMethodId)
        {
            decimal totalValue = value;

            if (shippingMethodId.HasValue && shippingMethodId != 0)
            {
                var method = _shippingMethodRepository.GetShippingMethodById(shippingMethodId.Value);
                if (method != null)
                {
                    totalValue += method.Price;
                }
            }

            return Json(totalValue.ToString("C"));
        }
    }
}