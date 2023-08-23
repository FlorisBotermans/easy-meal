using System.Collections.Generic;
using System.Linq;
using EasyMeal_Core.DomainModel;
using EasyMeal_Core.DomainServices;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;

namespace EasyMeal_WebApi_Level2.Controllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        [HttpPost]
        public ActionResult<Customer> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            customerRepository.AddCustomer(customer);

            return CreatedAtAction(nameof(CreateCustomer), customer);
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            List<Customer> customers = customerRepository.Customers.ToList();
            List<HALResponse> result = new List<HALResponse>();

            if (customers == null) { return NotFound(); }

            foreach (Customer customer in customers)
            {
                result.Add(new HALResponse(new Customer
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    StreetName = customer.StreetName,
                    HouseNumber = customer.HouseNumber,
                    City = customer.City,
                    PostalCode = customer.PostalCode,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.Email,
                }).AddLinks(
                    new Link("self", "/api/customers"),
                    new Link("customers:orders", $"/api/customers/{customer.CustomerId}/orders")
                ));
            }

            return Ok(result);
        }

        [HttpGet("{customerId}")]
        public IActionResult GetCustomerById(int customerId)
        {
            Customer customer = customerRepository.GetCustomerById(customerId);
            if (customer == null) return NotFound();

            return this.HAL(new Customer
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                StreetName = customer.StreetName,
                HouseNumber = customer.HouseNumber,
                City = customer.City,
                PostalCode = customer.PostalCode,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
            }, new Link[]
            {
                new Link("self", $"/api/customers/{customerId}"),
                new Link("customers:orders", $"/api/customers/{customerId}/orders")
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
