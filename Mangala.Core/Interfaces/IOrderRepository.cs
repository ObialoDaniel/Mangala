using Mangala.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Interfaces
{
    public interface IOrderRepository
    {
        Order Add(Order order);
        Order? GetById(int id);
        List<Order> GetByCustomer(int customerId); 
    }
}