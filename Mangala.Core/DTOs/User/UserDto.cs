using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}