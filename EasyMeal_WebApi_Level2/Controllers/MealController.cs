using System.Collections.Generic;
using System.Linq;
using EasyMeal_Core.DomainModel;
using EF_SQLServer_Meal_DataStoreImpl;
using Microsoft.AspNetCore.Mvc;

namespace EasyMeal_WebApi_Level2.Controllers
{
    [Route("api/meals")]
    [ApiController]
    public class MealController : Controller
    {
        private MealDbContext mealDb = new MealDbContext();

        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetAllMeals()
        {
            List<Meal> meals = mealDb.Meals.ToList();

            if (meals == null) return NotFound();

            return Ok(meals);
        }
    }
}
