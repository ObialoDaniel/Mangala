using Mangala.Core.Common;
using Mangala.Core.DTOs;
using Mangala.Core.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Interfaces
{
    public interface IRoleService
    {
        ApiResponse<RoleDto> CreateRole(CreateRoleDto request);
        ApiResponse<List<RoleDto>> GetAllRoles();
    }
}
