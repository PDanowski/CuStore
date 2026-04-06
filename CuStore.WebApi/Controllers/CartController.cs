using CuStore.Core.Abstractions;
using CuStore.Core.Entities;
using CuStore.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CuStore.WebApi.Controllers;

[ApiController]
[Route("api/users/{userId}/cart")]
public class CartController(ICartRepository cartRepository, IProductRepository productRepository) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCart(string userId)
    {
        var cart = cartRepository.GetActiveCartWithItemsForUser(userId) ?? new Cart { UserId = userId };
        return Ok(cart);
    }

    [HttpPost("items")]
    public IActionResult AddToCart(string userId, [FromBody] AddToCartRequest request)
    {
        var product = productRepository.GetProductById(request.ProductId);
        if (product is null)
        {
            return NotFound($"Product {request.ProductId} not found.");
        }

        var cart = cartRepository.GetActiveCartWithItemsForUser(userId);
        if (cart is null)
        {
            cart = new Cart { UserId = userId };
            cartRepository.AddCart(cart);
            cart = cartRepository.GetActiveCartWithItemsForUser(userId) ?? cart;
        }

        cart.AddProduct(product, Math.Max(1, request.Quantity));
        var saved = cartRepository.SaveCart(cart);

        return saved ? Ok(cart) : StatusCode(500, "Failed to save cart.");
    }

    [HttpDelete("items/{productId:int}")]
    public IActionResult RemoveFromCart(string userId, int productId)
    {
        var cart = cartRepository.GetActiveCartWithItemsForUser(userId);
        if (cart is null)
        {
            return NotFound("Cart not found.");
        }

        cart.RemoveProduct(productId);

        if (!cart.CartItems.Any())
        {
            cartRepository.RemoveCart(cart);
            return NoContent();
        }

        var saved = cartRepository.SaveCart(cart);
        return saved ? Ok(cart) : StatusCode(500, "Failed to save cart.");
    }
}
