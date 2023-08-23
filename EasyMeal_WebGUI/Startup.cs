using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using EasyMeal_WebGUI.Models;
using EasyMeal_Core.DomainServices;
using EF_SQLServer_Order_DataStoreImpl;
using EF_SQLServer_Identity_DataStoreImpl;
using Microsoft.AspNetCore.Identity;

namespace EasyMeal_WebGUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OrderDbContext>();
            services.AddDbContext<IdentityDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddTransient<ICustomerRepository, EFCustomerRepository>();
            services.AddScoped(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "meal",
                    template: "{controller=Meal}/{action=Index}/{id?}");
            });

            OrderSeedData.EnsurePopulated(app);
        }
    }
}