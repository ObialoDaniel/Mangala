using Mangala.Core.Common;
using Mangala.Core.DTOs.Auth;
using Mangala.Core.DTOs.Register;
using Mangala.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Interfaces
{
    public interface IAuthService
    {
        ApiResponse<LoginResponseDto> Login(LoginDto request);
        Task<ApiResponse<UserDto>> Register(RegisterDTO request);
        ApiResponse<LoginResponseDto> LoginWithGoogle(string email, string fullName);
    }
}
