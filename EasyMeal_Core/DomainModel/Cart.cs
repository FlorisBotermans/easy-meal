using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace EasyMeal_Core.DomainModel
{
    public class Cart
    {
        private List<CartLine> lines = new List<CartLine>();

        public virtual void AddItem(int mealId, string size, bool isStarter, bool isDessert)
        {
            CartLine line = lines.FirstOrDefault(l => l.MealId == mealId);

            if (line == null)
            {
                lines.Add(new CartLine
                {
                    MealId = mealId,
                    Size = size,
                    IsStarter = isStarter,
                    IsDessert = isDessert
                });
            }
        }

        public virtual void RemoveLine(Meal meal) => lines.RemoveAll(l => l.MealId == meal.MealId);

        public virtual double ComputeTotalValue()
        {
            string BASE_URL = "http://localhost:58650/api/meals/";
            HttpClient client = new HttpClient(); client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            double totalValue = 0;

            foreach (CartLine line in lines)
            {
                Meal meal = client.GetAsync("").Result.Content.ReadAsAsync<List<Meal>>().Result
                .FirstOrDefault(m => m.MealId == line.MealId);

                if (line.Size == "Klein (-20%)") { meal.Price = meal.Price * 0.8; }
                else if (line.Size == "Groot (+20%)") { meal.Price = meal.Price * 1.2; }

                totalValue += meal.Price;
            }

            return totalValue;
        }

        public virtual void Clear() => lines.Clear();

        public virtual void EditLines(List<CartLine> cartLines) => lines = cartLines;

        public virtual List<CartLine> Lines => lines;
    }

    public class CartLine
    {
        [Key]
        public int CartLineId { get; set; }

        public int MealId { get; set; }

        public string Size { get; set; }

        public bool IsStarter { get; set; }

        public bool IsDessert { get; set; }
    }
}
