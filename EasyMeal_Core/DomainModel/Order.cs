using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EasyMeal_Core.DomainModel
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }

        public virtual List<CartLine> Lines { get; set; }
    }
}