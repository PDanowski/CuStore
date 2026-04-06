using CuStore.Core.Entities;

namespace CuStore.Core.Abstractions;

public interface IShippingMethodRepository
{
    IEnumerable<ShippingMethod> GetShippingMethods();
    ShippingMethod? GetShippingMethodById(int id);
    bool SaveShippingMethod(ShippingMethod shippingMethod);
    bool AddShippingMethod(ShippingMethod shippingMethod);
    bool RemoveShippingMethod(int shippingMethodId);
}
