using CuStore.Core.Abstractions;
using CuStore.Core.Entities;
using CuStore.Infrastructure.Data;

namespace CuStore.Infrastructure.Repositories;

public class ShippingMethodRepository(StoreDbContext context) : IShippingMethodRepository
{
    public IEnumerable<ShippingMethod> GetShippingMethods() => context.ShippingMethods.ToList();

    public ShippingMethod? GetShippingMethodById(int id) =>
        context.ShippingMethods.FirstOrDefault(s => s.Id == id);

    public bool SaveShippingMethod(ShippingMethod shippingMethod)
    {
        var existingShippingMethod = context.ShippingMethods
            .SingleOrDefault(s => s.Id == shippingMethod.Id);

        if (existingShippingMethod is null)
        {
            return false;
        }

        context.Entry(existingShippingMethod).CurrentValues.SetValues(shippingMethod);
        context.SaveChanges();
        return true;
    }

    public bool AddShippingMethod(ShippingMethod shippingMethod)
    {
        context.ShippingMethods.Add(shippingMethod);
        context.SaveChanges();
        return true;
    }

    public bool RemoveShippingMethod(int shippingMethodId)
    {
        var shippingMethod = context.ShippingMethods.SingleOrDefault(s => s.Id == shippingMethodId);
        if (shippingMethod is null)
        {
            return false;
        }

        context.ShippingMethods.Remove(shippingMethod);
        context.SaveChanges();
        return true;
    }
}
