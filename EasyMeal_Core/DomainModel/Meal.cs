using System;

namespace EasyMeal_Core.DomainModel
{
    public class Meal
    {
        public int MealId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Starter { get; set; }

        public string Dessert { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }
    }
}