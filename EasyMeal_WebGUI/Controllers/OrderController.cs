using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using EasyMeal_Core.DomainModel;
using EasyMeal_Core.DomainServices;
using EasyMeal_WebGUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EasyMeal_WebGUI.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository orderRepository;
        private Cart cart;
        private string BASE_URL = "http://localhost:58650/api/meals/";
        private HttpClient client;

        public OrderController(IOrderRepository orderRepository, Cart cart)
        {
            this.orderRepository = orderRepository;
            this.cart = cart;
            client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public ViewResult Index()
        {
            List<MealSizeViewModel> mealSizeViewModels = new List<MealSizeViewModel>();

            Order order = orderRepository.Orders.Last();

            foreach (CartLine cartLine in order.Lines)
            {
                Meal meal = client.GetAsync("").Result.Content.ReadAsAsync<List<Meal>>().Result
                .FirstOrDefault(m => m.MealId == cartLine.MealId);

                if (!cartLine.IsStarter) { meal.Starter = null; }
                if (!cartLine.IsDessert) { meal.Dessert = null; }

                if (cartLine.Size == "Klein (-20%)") { meal.Price = meal.Price * 0.8; }
                else if (cartLine.Size == "Groot (+20%)") { meal.Price = meal.Price * 1.2; }

                MealSizeViewModel mealSizeviewModel = new MealSizeViewModel()
                {
                    Meal = meal,
                    Size = cartLine.Size
                };

                mealSizeViewModels.Add(mealSizeviewModel);
            }

            return View(mealSizeViewModels);
        }

        [HttpPost]
        [Authorize]
        public IActionResult SaveOrder()
        {
            Order order = new Order();
            order.Lines = cart.Lines;

            orderRepository.SaveOrder(order);

            cart.Clear();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditOrder(List<Meal> meals)
        {
            return View(meals);
        }

        //[HttpPost]
        //public IActionResult EditOrder()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        orderRepository.EditOrder(order);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {
        //        return RedirectToAction(nameof(OrderLine));
        //    }
        //}
    }
}