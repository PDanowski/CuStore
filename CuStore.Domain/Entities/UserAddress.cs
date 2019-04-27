using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.Domain.Entities
{
    public class UserAddress : Address
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
