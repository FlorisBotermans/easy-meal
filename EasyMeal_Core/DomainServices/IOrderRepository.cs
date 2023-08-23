using System.Linq;
using EasyMeal_Core.DomainModel;

namespace EasyMeal_Core.DomainServices
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
        Order GetOrderById(int orderId);
        void DeleteOrder(int orderId);
        void EditOrder(Order order);
    }
}