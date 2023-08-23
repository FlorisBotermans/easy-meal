using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using EasyMeal_Core.DomainModel;
using EasyMeal_WebGUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EasyMeal_WebGUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private Cart cart;
        private string BASE_URL = "http://localhost:58650/api/meals/";
        private HttpClient client;

        public CartController(Cart cart)
        {
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

            foreach (CartLine cartLine in cart.Lines)
            {
                Meal meal = client.GetAsync("").Result.Content.ReadAsAsync<List<Meal>>().Result
                .FirstOrDefault(m => m.MealId == cartLine.MealId);

                if (!cartLine.IsStarter) { meal.Starter = null; }
                if (!cartLine.IsDessert) { meal.Dessert = null; }

                if (cartLine.Size == "Klein (-20%)") { meal.Price = meal.Price * 0.8; }
                else if (cartLine.Size == "Groot (+20%)") { meal.Price = meal.Price * 1.2; }

                MealSizeViewModel mealSizeViewModel = new MealSizeViewModel()
                {
                    Meal = meal,
                    Size = cartLine.Size
                };

                mealSizeViewModels.Add(mealSizeViewModel);
            }

            return View(mealSizeViewModels);
        }

        public RedirectToActionResult AddToCart(int mealId, string size, bool isStarter, bool isDessert)
        {
            Meal meal = client.GetAsync("").Result.Content.ReadAsAsync<List<Meal>>().Result
                .FirstOrDefault(m => m.MealId == mealId);

            bool isAdded = false;
            foreach (CartLine cartLine in cart.Lines)
            {
                if(cartLine.MealId == meal.MealId)
                {
                    isAdded = true;
                }
            }

            if (!isAdded)
            {
                cart.AddItem(meal.MealId, size, isStarter, isDessert);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Meal");
        }

        public RedirectToActionResult RemoveFromCart(int mealId)
        {
            Meal meal = client.GetAsync("").Result.Content.ReadAsAsync<List<Meal>>().Result
                .FirstOrDefault(m => m.MealId == mealId);

            if (meal != null)
            {
                cart.RemoveLine(meal);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
