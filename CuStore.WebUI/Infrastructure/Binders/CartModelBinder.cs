﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuStore.Domain.Abstract;
using CuStore.Domain.Concrete;
using CuStore.Domain.Entities;
using CuStore.Domain.Repositories;
using Microsoft.AspNet.Identity;
using Ninject;

namespace CuStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string SessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = (Cart) controllerContext.HttpContext.Session[SessionKey];

            if (controllerContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var repository = NinjectContainer.Resolve<CartRepository>();
                string userId = controllerContext.HttpContext.User.Identity.GetUserId();

                var userCart = repository.GetCartByUserId(userId);

                if (userCart == null)
                {
                    cart = new Cart {UserId = userId};
                    repository.AddCart(cart);
                }
                else
                {
                    controllerContext.HttpContext.Session[SessionKey] = userCart;
                    return userCart;
                }
            }
            else if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[SessionKey] = cart;
            }

            return cart;
        }
    }
}