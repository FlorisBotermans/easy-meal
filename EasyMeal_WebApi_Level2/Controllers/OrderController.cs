using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyMeal_Core.DomainModel;
using EasyMeal_Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace EasyMeal_WebApi_Level2.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : Controller
    {
        private IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            List<Order> orders = orderRepository.Orders.ToList();
            if (orders == null) { return NotFound(); }

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrderById(int orderId)
        {
            Order order = orderRepository.GetOrderById(orderId);
            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpDelete("{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            orderRepository.DeleteOrder(orderId);
            return Ok(new { Message = "Order successfully deleted." });
        }
    }
}
