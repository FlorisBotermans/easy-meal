using EasyMeal_Core.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace EF_SQLServer_Meal_DataStoreImpl
{
    public class MealDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(@"C:\Users\Floris\Desktop\EasyMeal_OrderApplication\EasyMeal_WebGUI")
                                    .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration["Data:EasyMealMealDb:ConnectionString"]);
        }

        public DbSet<Meal> Meals { get; set; }
    }
}
