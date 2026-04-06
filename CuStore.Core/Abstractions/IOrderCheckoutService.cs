using CuStore.Core.Entities;

namespace CuStore.Core.Abstractions;

public interface IOrderCheckoutService
{
    Task<Order?> CheckoutAsync(string userId, int shippingMethodId, bool useUserAddress, ShippingAddress? shippingAddress, CancellationToken cancellationToken = default);
}
