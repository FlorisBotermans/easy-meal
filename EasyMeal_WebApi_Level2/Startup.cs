using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using EasyMeal_Core.DomainServices;
using EF_SQLServer_Order_DataStoreImpl;
using Swashbuckle.AspNetCore.Swagger;

namespace EasyMeal_WebApi_Level2
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

            services.AddScoped<ICustomerRepository, EFCustomerRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "EasyMeal_WebApi_Level2", Version = "v1" }); });

            services.AddMvc();
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyMeal_WebApi_Level2"); });
            app.UseMvc();
        }
    }
}
