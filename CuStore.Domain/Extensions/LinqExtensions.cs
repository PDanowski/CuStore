using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.Domain.Extensions
{
    public static class LinqExtensions
    {
        public static bool In (this int data, IEnumerable<int> values) 
        {
            foreach (var val in values)
            {
                if (val.Equals(data))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
