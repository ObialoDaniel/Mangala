using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mangala.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User User { get; set; }
    }
}
