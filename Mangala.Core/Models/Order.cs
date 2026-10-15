using System;
using System.Collections.Generic;
using Mangala.Core.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mangala.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public User User { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DateCreated { get; set; }   
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public PaymentStatus PaymentStatus { get; set; }
    }
}

//public int Quantity { get; set; }
//public int MenuItemId { get; set; }
//[ForeignKey(nameof(MenuItemId))]
//public MenuItem MenuItem { get; set; }