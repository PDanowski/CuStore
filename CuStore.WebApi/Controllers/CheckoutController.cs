using CuStore.Core.Abstractions;
using CuStore.Core.Entities;
using CuStore.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CuStore.WebApi.Controllers;

[ApiController]
[Route("api/users/{userId}")]
public class CheckoutController(IOrderCheckoutService checkoutService, IUserRepository userRepository, ICrmClient crmClient) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(string userId, [FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(userId);
        if (user is null)
        {
            user = new User
            {
                Id = userId,
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
            };

            userRepository.AddUser(user);
        }

        if (!user.CrmGuid.HasValue || user.CrmGuid == Guid.Empty)
        {
            user.CrmGuid = await crmClient.CreateCustomerDataAsync(user.UserName, 0, cancellationToken);
            userRepository.UpdateUsers(new[] { user });
        }

        return Ok(user);
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(string userId, [FromBody] CheckoutRequest request, CancellationToken cancellationToken)
    {
        ShippingAddress? shippingAddress = null;
        if (!request.UseUserAddress)
        {
            shippingAddress = new ShippingAddress
            {
                FirstName = request.FirstName ?? string.Empty,
                LastName = request.LastName ?? string.Empty,
                Phone = request.Phone ?? string.Empty,
                Street = request.Street ?? string.Empty,
                City = request.City ?? string.Empty,
                Country = request.Country ?? string.Empty,
                PostalCode = request.PostalCode ?? string.Empty,
            };
        }

        var order = await checkoutService.CheckoutAsync(
            userId,
            request.ShippingMethodId,
            request.UseUserAddress,
            shippingAddress,
            cancellationToken);

        return order is null ? BadRequest("Unable to checkout: empty cart or invalid state.") : Ok(order);
    }
}
