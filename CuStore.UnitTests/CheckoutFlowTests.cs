using CuStore.Core.Abstractions;
using CuStore.Core.Entities;
using CuStore.Infrastructure.Data;
using CuStore.Infrastructure.Repositories;
using CuStore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CuStore.Net10.Tests;

public class CheckoutFlowTests
{
    [Fact]
    public async Task Checkout_AddsOrderAndCallsCrm()
    {
        var options = new DbContextOptionsBuilder<StoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var db = new StoreDbContext(options);

        var category = new Category { Id = 1, Name = "Cat" };
        var product = new Product { Id = 1, Name = "Prod", Description = "x", Code = "X", Price = 100, QuanityInStock = 3, CategoryId = 1 };
        var shipping = new ShippingMethod { Id = 1, Name = "Courier", Price = 10, MaximumPcs = 100 };
        var user = new User { Id = "u1", UserName = "u1", Email = "u1@x", FirstName = "A", LastName = "B", BirthDate = DateTime.UtcNow, CrmGuid = Guid.NewGuid() };
        var cart = new Cart { Id = 1, UserId = user.Id };
        cart.AddProduct(product, 1);

        db.Categories.Add(category);
        db.Products.Add(product);
        db.ShippingMethods.Add(shipping);
        db.Users.Add(user);
        db.Carts.Add(cart);
        db.SaveChanges();

        var cartRepo = new CartRepository(db);
        var shippingRepo = new ShippingMethodRepository(db);
        var orderRepo = new OrderRepository(db);
        var userRepo = new UserRepository(db);
        var emailSender = new NoOpEmailSender();
        var crm = new FakeCrmClient();

        var checkout = new OrderCheckoutService(cartRepo, shippingRepo, orderRepo, userRepo, emailSender, crm);

        var order = await checkout.CheckoutAsync(user.Id, 1, true, null);

        Assert.NotNull(order);
        Assert.Equal(1, db.Orders.Count());
        Assert.True(crm.LastPoints > 0);
    }

    private sealed class FakeCrmClient : ICrmClient
    {
        public int LastPoints { get; private set; }

        public Task<Guid> CreateCustomerDataAsync(string customerCode, int bonusPoints, CancellationToken cancellationToken = default)
            => Task.FromResult(Guid.NewGuid());

        public Task<bool> AddPointsForCustomerAsync(Guid customerGuid, int points, CancellationToken cancellationToken = default)
        {
            LastPoints = points;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveCustomerDataAsync(Guid customerGuid, CancellationToken cancellationToken = default)
            => Task.FromResult(true);

        public Task<int> GetPointsForCustomerAsync(Guid customerGuid, CancellationToken cancellationToken = default)
            => Task.FromResult(LastPoints);
    }
}
