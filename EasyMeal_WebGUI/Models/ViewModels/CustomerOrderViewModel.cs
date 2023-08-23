using EasyMeal_Core.DomainModel;
using System.Collections.Generic;

namespace EasyMeal_WebGUI.Models.ViewModels
{
    public class CustomerOrderViewModel
    {
        public Customer Customer { get; set; }
        public List<MealSizeViewModel> MealSizeViewModels { get; set; }
    }
}