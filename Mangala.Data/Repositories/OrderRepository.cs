using Mangala.Core.Interfaces;
using Mangala.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Data.Repositories
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        public Order Add(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
            return order;
        }

        public Order? GetById(int id)
        {
            return context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetByCustomer (int customerId)
        {
            return context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .Where(o => o.CustomerId == customerId)
                .ToList();
        }
    }
}


