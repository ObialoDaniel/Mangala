using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.DTOs.MenuItem
{
    public class CreateMenuItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }   
        public DateOnly AvailableDate { get; set; }
    }

    public class MenuItemDto
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateOnly AvailableDate { get; set; }
    }
}
