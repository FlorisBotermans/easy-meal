using System.ComponentModel.DataAnnotations;

namespace EasyMeal_Core.DomainModel
{
    public class CustomerOrder
    {
        [Key]
        public int CustomerOrderId { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
