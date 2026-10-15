using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mangala.Core.DTOs.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Username cannot contain whitespace.")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Password cannot contain whitespace.")]
        public string Password { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "A valid role ID is required.")]
        public int RoleId { get; set; }
    }
}
