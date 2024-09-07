using ShoppingPlatform.BLL.Dto.RoleDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IRoleService
{
    Task<ApiResponse<List<RoleGetDto>>> GetAllRoles();
    Task<ApiResponse<RoleGetDto>> GetRoleById(int id);
    Task<ApiResponse<bool>> CreateRole(string roleName);
    Task<ApiResponse<bool>> DeleteRole(int id);
}