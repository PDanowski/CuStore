using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.Domain.Entities
{
    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }


        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }


    }
}
