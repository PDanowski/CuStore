using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Entities;

namespace CuStore.WebUI.ViewModels
{
    public class OrderDetailsViewModel
    {
        public Order Order { get; set; }
        public List<SelectListItem> ShippingMethods { get; set; }
        public decimal OrderValue { get; set; }
        public int SelectedShippingMethodId { get; set; }

        public void SetShippingMethods(List<ShippingMethod> shippingMethods)
        {
            foreach (var shippingMethod in shippingMethods)
            {
                ShippingMethods.Add(new SelectListItem
                {
                    Text = shippingMethod.Name + @" - " + shippingMethod.Price.ToString("C"),
                    Value = shippingMethod.Id.ToString()
                });
            }
        }
    }
}