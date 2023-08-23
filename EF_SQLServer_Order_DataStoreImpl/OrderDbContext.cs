using EasyMeal_Core.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF_SQLServer_Order_DataStoreImpl
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(@"C:\Users\Floris\Desktop\EasyMeal_OrderApplication\EasyMeal_WebGUI")
                                    .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration["Data:EasyMealOrderDb:ConnectionString"]).UseLazyLoadingProxies();
        }
    }
}