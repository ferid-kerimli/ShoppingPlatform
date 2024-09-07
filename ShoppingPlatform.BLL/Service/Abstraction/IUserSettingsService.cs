using ShoppingPlatform.BLL.Dto.UserDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IUserSettingsService
{
    Task<ApiResponse<bool>> UpdatePassword(UpdatePasswordDto updatePasswordDto);
    Task<ApiResponse<bool>> ChangeUsername(ChangeUsernameDto changeUsernameDto);
    Task<ApiResponse<bool>> ChangeEmail(ChangeEmailDto changeEmailDto);
}