using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mangala.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

    }
}
