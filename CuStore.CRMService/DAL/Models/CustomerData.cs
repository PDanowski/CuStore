using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CuStore.CRMService.DAL.Models
{
    public class CustomerCrmData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ExternalCode { get; set; }
        public int Points { get; set; }
        public decimal Ratio { get; set; }
    }
}