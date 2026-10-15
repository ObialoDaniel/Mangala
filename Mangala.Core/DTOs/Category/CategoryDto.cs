using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.DTOs.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
    }
}
