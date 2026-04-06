using CuStore.WebUI.Api;
using CuStore.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuStore.WebUI.Controllers;

public class StoreController(
    StoreApiClient apiClient) : Controller
{
    private const string DemoUserId = "demo-user";

    [HttpGet]
    public async Task<IActionResult> Index(int? categoryId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetProductsAsync(categoryId, pageNumber, pageSize, cancellationToken);

        var vm = new StoreIndexViewModel
        {
            Products = response.Items,
            CategoryId = categoryId,
            PageNumber = response.PageNumber,
            PageSize = response.PageSize,
            TotalItems = response.Total,
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1, CancellationToken cancellationToken = default)
    {
        await apiClient.AddToCartAsync(DemoUserId, productId, quantity, cancellationToken);
        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId, CancellationToken cancellationToken = default)
    {
        await apiClient.RemoveFromCartAsync(DemoUserId, productId, cancellationToken);
        return RedirectToAction(nameof(Cart));
    }

    [HttpGet]
    public async Task<IActionResult> Cart(int? shippingMethodId, CancellationToken cancellationToken = default)
    {
        var vm = await BuildCartViewModelAsync(shippingMethodId, cancellationToken);
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(int shippingMethodId, CancellationToken cancellationToken)
    {
        var cart = await apiClient.GetCartAsync(DemoUserId, cancellationToken);
        if (cart.Items.Count == 0)
        {
            TempData["Error"] = "Cart is empty.";
            return RedirectToAction(nameof(Cart));
        }

        await apiClient.EnsureDemoUserAsync(DemoUserId, cancellationToken);
        var order = await apiClient.CheckoutAsync(DemoUserId, shippingMethodId, cancellationToken);

        if (order is null)
        {
            TempData["Error"] = "Checkout failed.";
            return RedirectToAction(nameof(Cart));
        }

        return RedirectToAction(nameof(Completed), new { id = order.OrderId });
    }

    [HttpGet]
    public IActionResult Completed(int id) => View(model: id);

    private async Task<CartViewModel> BuildCartViewModelAsync(int? shippingMethodId, CancellationToken cancellationToken)
    {
        var cart = await apiClient.GetCartAsync(DemoUserId, cancellationToken);
        var shippingMethods = (await apiClient.GetShippingMethodsAsync(cancellationToken)).ToList();

        var lines = cart.Items
            .Select(item => new CartDisplayItem
            {
                ProductId = item.ProductId,
                Name = item.Name,
                Quantity = item.Quantity,
                Price = item.Price,
            })
            .ToList();

        var subtotal = cart.Subtotal;
        var selectedShipping = shippingMethodId.HasValue
            ? shippingMethods.FirstOrDefault(s => s.Id == shippingMethodId.Value)
            : shippingMethods.FirstOrDefault();

        var shippingPrice = selectedShipping?.Price ?? 0;

        return new CartViewModel
        {
            Items = lines,
            Subtotal = subtotal,
            ShippingMethods = shippingMethods,
            SelectedShippingMethodId = selectedShipping?.Id ?? 0,
            ShippingPrice = shippingPrice,
            Total = subtotal + shippingPrice,
        };
    }
}
