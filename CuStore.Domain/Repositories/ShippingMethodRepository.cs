using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Repositories
{
    public class ShippingMethodRepository : IShippingMethodRepository
    {
        private readonly IStoreContext _context;

        public ShippingMethodRepository(IStoreContext context)
        {
            _context = context;
        }

        public ShippingMethod GetShippingMethodById(int id)
        {
            return _context.ShippingMethods.FirstOrDefault(s => s.Id.Equals(id));
        }

        public bool SaveShippingMethod(ShippingMethod shippingMethod)
        {
            try
            {
                var existingShippingMethod = _context.ShippingMethods
                    .SingleOrDefault(s => s.Id == shippingMethod.Id);

                // Update 
                _context.Entry(existingShippingMethod).CurrentValues.SetValues(shippingMethod);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public bool AddShippingMethod(ShippingMethod shippingMethod)
        {
            try
            {
                _context.ShippingMethods.Add(shippingMethod);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public bool RemoveShippingMethod(int shippingMethodId)
        {
            try
            {
                ShippingMethod shippingMethod = new ShippingMethod { Id = shippingMethodId };
                _context.Entry(shippingMethod).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public IEnumerable<ShippingMethod> GetShippingMethods()
        {
            return _context.ShippingMethods.ToList();
        }
    }
}
