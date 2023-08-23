using System.Collections.Generic;
using System.Linq;
using EasyMeal_Core.DomainModel;

namespace EasyMeal_Core.DomainServices
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> Customers { get; }
        void AddCustomer(Customer customer);
        Customer GetCustomerById(int customerId);
        void DeleteCustomer(int customerId);
        void AddOrderToCustomer(int customerId, Order order);
    }
}
