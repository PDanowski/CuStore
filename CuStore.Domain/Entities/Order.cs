using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Entities.Enums;

namespace CuStore.Domain.Entities
{
    public class Order
    {

        public Order(Cart cart, bool useUserAddress, int shippingMethodId, ShippingAddress shippingAddress = null)
        {
            CartId = cart.Id;
            Cart = cart;
            OrderDate = DateTime.Now;
            ShippingAddress = shippingAddress;
            ShippingMethodId = shippingMethodId;
            UseUserAddress = useUserAddress;
        }

        public Order()
        {
            
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Order date")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }
        public bool UseUserAddress { get; set; }
        public OrderStatus Status { get; set; }

        public int CartId { get; set; }
        public int ShippingMethodId { get; set; }
        public int? ShippingAddressId { get; set; }
        public virtual ShippingAddress ShippingAddress { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ShippingMethod ShippingMethod { get; set; }


        public decimal GetTotalValue()
        {
            decimal totalValue = 0;

            if (Cart != null)
            {
                totalValue += Cart.GetValue();
            }
            if (ShippingMethod != null)
            {
                totalValue += ShippingMethod.Price;
            }

            return totalValue;
        }


    }
}
