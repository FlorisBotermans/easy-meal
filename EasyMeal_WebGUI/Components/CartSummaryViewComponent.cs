using Microsoft.AspNetCore.Mvc;
using EasyMeal_Core.DomainModel;

namespace EasyMeal_WebGUI.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private Cart cart;

        public CartSummaryViewComponent(Cart cart)
        {
            this.cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}