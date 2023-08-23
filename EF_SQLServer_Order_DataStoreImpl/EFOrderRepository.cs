using System.Linq;
using EasyMeal_Core.DomainModel;
using EasyMeal_Core.DomainServices;

namespace EF_SQLServer_Order_DataStoreImpl
{
    public class EFOrderRepository : IOrderRepository
    {
        public OrderDbContext context;

        public EFOrderRepository(OrderDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Order> Orders => context.Orders;

        public void SaveOrder(Order order)
        {
            context.Orders.Add(order);

            context.SaveChanges();
        }

        public Order GetOrderById(int orderId)
        {
            return context.Orders.Find(orderId);
        }

        public void DeleteOrder(int orderId)
        {
            Order order = context.Orders.Find(orderId);
            if (order != null)
            {
                context.Orders.Remove(order);
                context.SaveChanges();
            }
        }

        public void EditOrder(Order order)
        {
            Order orderToChange = context.Orders.Single(o => o.OrderId == order.OrderId);

            context.Orders.Update(order);
            context.SaveChanges();
        }
    }
}