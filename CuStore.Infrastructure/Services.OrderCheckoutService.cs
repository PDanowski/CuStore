using CuStore.Core.Abstractions;
using CuStore.Core.Entities;

namespace CuStore.Infrastructure.Services;

public class OrderCheckoutService(
    ICartRepository cartRepository,
    IShippingMethodRepository shippingMethodRepository,
    IOrderRepository orderRepository,
    IUserRepository userRepository,
    IEmailSender emailSender,
    ICrmClient crmClient) : IOrderCheckoutService
{
    public async Task<Order?> CheckoutAsync(
        string userId,
        int shippingMethodId,
        bool useUserAddress,
        ShippingAddress? shippingAddress,
        CancellationToken cancellationToken = default)
    {
        var cart = cartRepository.GetActiveCartWithItemsForUser(userId);
        if (cart is null || cart.CartItems.Count == 0)
        {
            return null;
        }

        var order = new Order(cart, useUserAddress, shippingMethodId, !useUserAddress ? shippingAddress : null)
        {
            ShippingMethod = shippingMethodRepository.GetShippingMethodById(shippingMethodId),
        };

        if (!orderRepository.AddOrder(order))
        {
            return null;
        }

        var crmGuid = userRepository.GetUserById(userId)?.CrmGuid;
        if (crmGuid.HasValue && crmGuid.Value != Guid.Empty)
        {
            await crmClient.AddPointsForCustomerAsync(crmGuid.Value, (int)order.GetTotalValue(), cancellationToken);
        }

        emailSender.ProcessOrder(order);
        return order;
    }
}
