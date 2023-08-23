using System.Collections.Generic;
using System.Linq;
using EasyMeal_Core.DomainModel;
using EasyMeal_Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace EasyMeal_WebApi_Level2.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : Controller
    {
        private ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            List<Customer> customers = customerRepository.Customers.ToList();
            if (customers == null) { return NotFound(); }

            return Ok(customers);
        }

        [HttpGet("{customerId}")]
        public IActionResult GetCustomerById(int customerId)
        {
            Customer customer = customerRepository.GetCustomerById(customerId);
            if (customer == null) return NotFound();

            return Ok(new Customer {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                StreetName = customer.StreetName,
                HouseNumber = customer.HouseNumber,
                City = customer.City,
                PostalCode = customer.PostalCode,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email
            });
        }

        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomer(int customerId)
        {
            customerRepository.DeleteCustomer(customerId);
            return Ok(new { Message = "Customer successfully deleted." });
        }

        [HttpGet("{customerId}/orders")]
        public IActionResult GetCustomerOrders(int customerId)
        {
            Customer customer = customerRepository.GetCustomerById(customerId);

            List<Order> orders = new List<Order>();
            foreach (var customerOrder in customer.Orders)
            {
                orders.Add(customerOrder.Order);
            }

            if (orders == null) return NotFound();

            return Ok(orders);
        }
    }
}
