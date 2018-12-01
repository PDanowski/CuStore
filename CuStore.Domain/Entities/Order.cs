using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.Domain.Entities
{
    public class Order
    {
        public Order(Cart cart)
        {
            CartId = cart.Id;
            Cart = cart;
            OrderDate = DateTime.Now;
            Address = new Address();
        }

        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }
        public int ShippingMethodId { get; set; }
        public DateTime OrderDate { get; set; }

        public bool UseUserAddress { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public Cart Cart { get; set; }
        public ShippingMethod ShippingMethod { get; set; }


    }
}
