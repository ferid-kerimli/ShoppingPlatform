using ShoppingPlatform.BLL.Dto.UserDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IUserService
{
    Task<ApiResponse<List<UserGetDto>>> GetAllUsers();
    Task<ApiResponse<UserGetDto>> GetUserById(int id);
    Task<ApiResponse<bool>> DeleteUser(int id);
}