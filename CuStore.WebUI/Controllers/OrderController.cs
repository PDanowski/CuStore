using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CuStore.Domain.Abstract;

namespace CuStore.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        public ViewResult OrderDetails(int orderId)
        {
            return View(_orderRepository.GetOrderById(orderId));
        }
    }
}