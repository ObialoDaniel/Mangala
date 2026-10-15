using Mangala.Core.Common;
using Mangala.Core.DTOs;
using Mangala.Core.DTOs.Role;
using Mangala.Core.Interfaces;
using Mangala.Core.Models;

namespace Mangala.Core.Services
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        public ApiResponse<RoleDto> CreateRole(CreateRoleDto request)
        {
            var role = new Role
            {
                Name = request.Name,
                Description = request.Description,
                DateCreated = DateTime.UtcNow
            };
            var created = roleRepository.Add(role);

            var dto = new RoleDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                DateCreated = created.DateCreated
            };
            return ApiResponse<RoleDto>.Success(dto, "Role created successfully");
        }

        public ApiResponse<List<RoleDto>> GetAllRoles()
        {
            var roles = roleRepository.GetAll()
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    DateCreated = r.DateCreated
                }).ToList();

            return ApiResponse<List<RoleDto>>.Success(roles);
        }
    }
}