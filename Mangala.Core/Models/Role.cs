using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
    }

}
