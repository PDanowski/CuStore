using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuStore.CRMService.DAL.Models
{
    public class CustomerData
    {
        public Guid Id { get; set; }
        public string ExternalCode { get; set; }
        public int Points { get; set; }
        public decimal Ratio { get; set; }
    }
}