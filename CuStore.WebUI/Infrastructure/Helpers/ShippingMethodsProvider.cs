using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Entities;

namespace CuStore.WebUI.Infrastructure.Helpers
{
    public class ShippingMethodsProvider
    {
        public static List<SelectListItem> CreateSelectList(List<ShippingMethod> shippingMethods)
        {
            List <SelectListItem> selectList = new List<SelectListItem>();

            foreach (var shippingMethod in shippingMethods)
            {
                selectList.Add(new SelectListItem
                {
                    Text = shippingMethod.Name + @" - " + shippingMethod.Price.ToString("C"),
                    Value = shippingMethod.Id.ToString()
                });
            }

            return selectList;
        }
    }
}