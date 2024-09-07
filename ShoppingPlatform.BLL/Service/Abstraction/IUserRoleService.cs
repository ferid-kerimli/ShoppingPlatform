using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IUserRoleService
{
    Task<ApiResponse<bool>> AssignRoleToUser(int userId, string roleName);
}