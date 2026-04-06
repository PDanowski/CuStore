using CuStore.Core.Entities;

namespace CuStore.Net10.Tests;

public class CartTests
{
    [Fact]
    public void AddProduct_NewItem_AddsToCart()
    {
        var cart = new Cart();
        var product = new Product { Id = 1, Name = "P1", Code = "C1", Price = 10, QuanityInStock = 5, Description = "d" };

        cart.AddProduct(product, 2);

        Assert.Single(cart.CartItems);
        Assert.Equal(20, cart.GetValue());
    }
}
