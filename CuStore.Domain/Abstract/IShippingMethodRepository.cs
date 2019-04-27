using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Abstract
{
    public interface IShippingMethodRepository
    {
        IEnumerable<ShippingMethod> GetShippingMethods();
        ShippingMethod GetShippingMethodById(int id);
        bool SaveShippingMethod(ShippingMethod shippingMethod);
        bool AddShippingMethod(ShippingMethod shippingMethod);
        bool RemoveShippingMethod(int shippingMethodId);
    }
}
