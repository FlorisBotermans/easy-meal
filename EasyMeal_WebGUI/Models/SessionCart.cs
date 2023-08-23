using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

using EasyMeal_WebGUI.Infrastructure;
using EasyMeal_Core.DomainModel;

namespace EasyMeal_WebGUI.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart")
                ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession Session { get; set; }

        public override void AddItem(int mealId, string size, bool isStarter, bool isDessert)
        {
            base.AddItem(mealId, size, isStarter, isDessert);
            Session.SetJson("Cart", this);
        }

        public override void RemoveLine(Meal meal)
        {
            base.RemoveLine(meal);
            Session.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
