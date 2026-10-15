using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mangala.Core.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public DateOnly AvailableDate { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User User { get; set; }
        public string? BachsProductId { get; set; }
    }
}
