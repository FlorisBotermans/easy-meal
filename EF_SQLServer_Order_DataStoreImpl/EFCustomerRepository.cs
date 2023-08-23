using System;
using System.Collections.Generic;
using System.Linq;
using EasyMeal_Core.DomainModel;
using EasyMeal_Core.DomainServices;

namespace EF_SQLServer_Order_DataStoreImpl
{
    public class EFCustomerRepository : ICustomerRepository
    {
        public OrderDbContext context;
        public EFCustomerRepository(OrderDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Customer> Customers => context.Customers;

        public void AddCustomer(Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public Customer GetCustomerById(int customerId)
        {
            return context.Customers.Find(customerId);
        }

        public void DeleteCustomer(int customerId)
        {
            Customer customer = context.Customers.Find(customerId);
            if (customer != null)
            {
                context.Customers.Remove(customer);
                context.SaveChanges();
            }
        }

        public void AddOrderToCustomer(int customerId, Order order)
        {
            Customer customer = context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

            if (customer != null)
            {
                CustomerOrder customerMeal = new CustomerOrder { Customer = customer, Order = order };

                customer.Orders.Add(customerMeal);
                context.SaveChanges();
            }
        }
    }
}
