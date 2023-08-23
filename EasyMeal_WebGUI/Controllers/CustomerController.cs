using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EasyMeal_Core.DomainModel;
using Microsoft.AspNetCore.Authorization;
using EasyMeal_Core.DomainServices;
using EasyMeal_WebGUI.Models.ViewModels;
using System.Net.Http;

namespace EasyMeal_WebGUI.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private Cart cart;
        private ICustomerRepository customerRepository;
        private IOrderRepository orderRepository;
        private string BASE_URL = "http://localhost:58650/api/meals/";
        private HttpClient client;

        public CustomerController(ICustomerRepository customerRepository, IOrderRepository orderRepository, Cart cart)
        {
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
            this.cart = cart;
            client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public IActionResult Detail(int customerId)
        {
            Customer customer = customerRepository.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            List<MealSizeViewModel> mealSizeViewModels = new List<MealSizeViewModel>();

            foreach (CartLine cartLine in customer.Orders.Last().Order.Lines)
            {
                Meal meal = client.GetAsync("").Result.Content.ReadAsAsync<List<Meal>>().Result
                .FirstOrDefault(m => m.MealId == cartLine.MealId);

                if (!cartLine.IsStarter) { meal.Starter = null; }
                if (!cartLine.IsDessert) { meal.Dessert = null; }

                if (cartLine.Size == "Klein (-20%)") { meal.Price = meal.Price * 0.8; }
                else if (cartLine.Size == "Groot (+20%)") { meal.Price = meal.Price * 1.2; }

                MealSizeViewModel mealSizeviewModel = new MealSizeViewModel()
                    { Meal = meal, Size = cartLine.Size };

                mealSizeViewModels.Add(mealSizeviewModel);
            }

            CustomerOrderViewModel customerOrderViewModel = new CustomerOrderViewModel()
                { Customer = customer, MealSizeViewModels = mealSizeViewModels };

            return View(customerOrderViewModel);
        }

        [HttpPost]
        public IActionResult AddOrderToCustomer()
        {
            Customer customer = customerRepository.Customers.FirstOrDefault(c => c.Email == User.Identity.Name);

            customerRepository.AddOrderToCustomer(customer.CustomerId, new Order { Lines = cart.Lines });

            cart.Clear();

            return RedirectToAction("Detail", new { customerId = customer.CustomerId });
        }
    }
}