using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace EF_SQLServer_Order_DataStoreImpl
{
    public static class OrderSeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            OrderDbContext context = app.ApplicationServices.GetRequiredService<OrderDbContext>();
            context.Database.Migrate();
        }
    }
}