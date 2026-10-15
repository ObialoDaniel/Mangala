using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.DTOs.MenuItem
{
    public class UpdateMenuItemDto
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
