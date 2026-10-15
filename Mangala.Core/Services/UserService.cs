using Mangala.Core.Common;
using Mangala.Core.DTOs.User;
using Mangala.Core.Interfaces;
using Mangala.Core.Models;

namespace Mangala.Core.Services
{
    public class UserService(IUserRepository userRepository, IRoleRepository roleRepository) : IUserService
    {
        public ApiResponse<UserDto> CreateUser(CreateUserDto request)
        {
            if (userRepository.GetByUsername(request.Username) is not null)
                return ApiResponse<UserDto>.Failure("Username already exists");

            var role = roleRepository.GetById(request.RoleId);
            if (role is null)
                return ApiResponse<UserDto>.Failure("Role does not exist");  

            var user = new User
            {
                Uid = Guid.NewGuid(),
                Username = request.Username,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = request.RoleId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = userRepository.Add(user);

            var dto = new UserDto
            {
                Id = created.Id,
                Uid = created.Uid,
                Username = created.Username,
                FullName = created.FullName,
                PhoneNumber = created.PhoneNumber,
                Email = created.Email,
                RoleName = created.Role?.Name ?? string.Empty,
                CreatedAt = created.CreatedAt
            };
            return ApiResponse<UserDto>.Success(dto, "User created successfully");
        }

        public ApiResponse<List<UserDto>> GetAllUsers()
        {
            var users = userRepository.GetAll()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Uid = u.Uid,
                    Username = u.Username,
                    FullName = u.FullName,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    RoleName = u.Role.Name,
                    CreatedAt = u.CreatedAt
                }).ToList();

            return ApiResponse<List<UserDto>>.Success(users);
        }

        public ApiResponse<UserDto> GetUserById(int id)
        {
            var user = userRepository.GetById(id);
            if (user is null)
                return ApiResponse<UserDto>.Failure("User not found");

            var dto = new UserDto
            {
                Id = user.Id,
                Uid = user.Uid,
                Username = user.Username,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                RoleName = user.Role.Name,
                CreatedAt = user.CreatedAt
            };
            return ApiResponse<UserDto>.Success(dto);
        }
    }
}