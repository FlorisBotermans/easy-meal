using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyMeal_Core.DomainModel;
using EasyMeal_Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace EasyMeal_WebApi_Level3.Controllers
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

        [HttpPut("{orderId}")]
        public IActionResult EditOrder(int orderId, [FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "One or more properties are incorrect." });
            }

            Order oldOrder = orderRepository.Orders.SingleOrDefault(o => o.OrderId == orderId);

            orderRepository.EditOrder(order);

            Order editedOrder = orderRepository.Orders.SingleOrDefault(o => o.OrderId == orderId);

            return Ok(editedOrder);
        }

        [HttpDelete("{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            orderRepository.DeleteOrder(orderId);
            return Ok(new { Message = "Order successfully deleted." });
        }
    }
}
