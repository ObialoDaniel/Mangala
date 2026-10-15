using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.DTOs.Role
{
   public class CreateRoleDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
