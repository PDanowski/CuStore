using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Entities;

namespace CuStore.WebUI.ViewModels
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            ShippingMethods = new List<SelectListItem>();
            ShippingAddress = new ShippingAddress();
        }

        public Cart Cart { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public List<SelectListItem> ShippingMethods { get; set; }
        public decimal OrderValue { get; set; }
        public bool UseUserAddress { get; set; }

        [Required(ErrorMessage = "Shipping method  is required")]
        public int SelectedShippingMethodId { get; set; }

    }
}