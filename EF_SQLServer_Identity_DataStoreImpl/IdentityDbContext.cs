using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EF_SQLServer_Identity_DataStoreImpl
{
    public class IdentityDbContext : IdentityDbContext<IdentityUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(@"C:\Users\Floris\Desktop\EasyMeal_OrderApplication\EasyMeal_WebGUI")
                                    .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration["Data:EasyMealIdentityDb:ConnectionString"]);
        }
    }
}