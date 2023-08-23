using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using EasyMeal_Core.DomainModel;
using EasyMeal_Core.DomainServices;
using System.Globalization;

namespace EasyMeal_WebGUI.Controllers
{
    public class MealController : Controller
    {
        private ICustomerRepository customerRepository;
        private string BASE_URL = "http://localhost:58650/api/meals/";
        private HttpClient client;

        public MealController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
            client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                Customer customer = customerRepository.Customers.FirstOrDefault(c => c.Email == User.Identity.Name);

                if (customer.Orders.Count > 0)
                {
                    CustomerOrder order = customer.Orders.Last();

                    Meal meal = client.GetAsync("").Result.Content.ReadAsAsync<List<Meal>>().Result.FirstOrDefault(m => m.MealId == order.Order.Lines.First().MealId);

                    int weekOfMeal = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(meal.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                    int weekOfToday = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

                    if (weekOfMeal == weekOfToday)
                    {
                        return RedirectToAction("Detail", "Customer", new { customerId = customer.CustomerId });
                    }
                } else
                {
                    return View(client.GetAsync("").Result.Content.ReadAsAsync<List<Meal>>().Result.OrderBy(m => m.Date));
                }
            }

            return View(client.GetAsync("").Result.Content.ReadAsAsync<List<Meal>>().Result.OrderBy(m => m.Date));
        }
    }
}