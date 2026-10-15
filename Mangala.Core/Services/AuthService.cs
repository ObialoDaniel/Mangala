using Mangala.Core.Common;
using Mangala.Core.Common.Mangala.Core.Common;
using Mangala.Core.DTOs.Auth;
using Mangala.Core.DTOs.Register;
using Mangala.Core.DTOs.User;
using Mangala.Core.Interfaces;
using Mangala.Core.Models;
using Mangala.Core.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mangala.Core.Services
{
    public class AuthService(IUserRepository userRepository, IRoleRepository roleRepository, JwtSettings jwtSettings, IEmailService emailService) : IAuthService
    {
        public ApiResponse<LoginResponseDto> Login(LoginDto request)
        {
            var user = userRepository.GetByUsername(request.Username);

            if (user is null || user.PasswordHash is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return ApiResponse<LoginResponseDto>.Failure("Invalid username or password.");

            return BuildTokenResponse(user);
        }

        public async Task<ApiResponse<UserDto>> Register(RegisterDTO request)
        {
            if (userRepository.GetByUsername(request.Username) is not null)
                return ApiResponse<UserDto>.Failure("Username is already taken");

            var role = roleRepository.GetByName("Customer");
            if (role is null)
                return ApiResponse<UserDto>.Failure("Default role is not configured");

            var user = new User
            {
                Uid = Guid.NewGuid(),
                Username = request.Username,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = role.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = userRepository.Add(user);
            await SendWelcomeEmail(created.Email, created.FullName);

            var dto = new UserDto
            {
                Id = created.Id,
                Uid = created.Uid,
                Username = created.Username,
                FullName = created.FullName,
                PhoneNumber = created.PhoneNumber,
                Email = created.Email,
                RoleName = role.Name,
                CreatedAt = created.CreatedAt
            };

            return ApiResponse<UserDto>.Success(dto, "Registration successful");
        }

        private async Task SendWelcomeEmail(string email, string fullName)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(fullName))
            {
                return;
            }
              

            await emailService.SendAsync(new EmailMessage
            {
                ToEmail = email,
                ToName = fullName,
                Subject = "Welcome to Mangala",
                HtmlContent = $"<p>Hi {fullName}," +
                    $"</p><p>" +
                    $"Welcome to Mangala! We're excited to have you on board.</p>"
            });
        }

        public ApiResponse<LoginResponseDto> LoginWithGoogle(string email, string fullName)
        {
            var user = userRepository.GetByEmail(email);

            if (user is null)
            {
                var role = roleRepository.GetByName("Customer");
                if (role is null)
                    return ApiResponse<LoginResponseDto>.Failure("Default role is not configured");

                user = new User
                {
                    Uid = Guid.NewGuid(),
                    Username = email,
                    FullName = fullName,
                    PhoneNumber = string.Empty,
                    Email = email,
                    PasswordHash = null,
                    RoleId = role.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                user = userRepository.Add(user);
                user.Role = role;  
            }

            return BuildTokenResponse(user);
        }

        private ApiResponse<LoginResponseDto> BuildTokenResponse(User user)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim("role", user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return ApiResponse<LoginResponseDto>.Success(new LoginResponseDto
            {
                Token = tokenString,
                ExpiresAt = expiresAt
            }, "Login successful");
        }
    }
}