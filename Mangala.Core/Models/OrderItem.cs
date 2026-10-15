using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mangala.Core.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
        public int MenuItemId { get; set; }
        [ForeignKey(nameof(MenuItemId))]
        public MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
