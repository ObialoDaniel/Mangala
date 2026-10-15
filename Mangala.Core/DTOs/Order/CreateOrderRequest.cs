
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mangala.Core.DTOs.Order
{
    public class CreateOrderRequest
    {
        [Required]
        [MinLength(1)]
        public List<CreateOrderItemRequest> Items { get; set; } = new();
    }

    public class CreateOrderItemRequest
    {
        [Range(1, int.MaxValue)]
        public int MenuItemId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}