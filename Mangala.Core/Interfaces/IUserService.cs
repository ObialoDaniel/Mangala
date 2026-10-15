using Mangala.Core.Common;
using Mangala.Core.DTOs.User;


namespace Mangala.Core.Interfaces
{
    public interface IUserService
    {
        ApiResponse<UserDto> CreateUser(CreateUserDto request);
        ApiResponse<List<UserDto>> GetAllUsers();
        ApiResponse<UserDto> GetUserById(int id);
    }
}
