using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingPlatform.BLL.Dto.UserDto;
using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.API.Controller;

[Route("api")]
[ApiController]
[Authorize]
public class UserSettingsController : ControllerBase
{
    private readonly IUserSettingsService _userSettingsService;

    public UserSettingsController(IUserSettingsService userSettingsService)
    {
        _userSettingsService = userSettingsService;
    }

    [HttpPut("UpdatePassword")]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordDto updatePasswordDto)
    {
        var result = await _userSettingsService.UpdatePassword(updatePasswordDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("UpdateUsername")]
    public async Task<IActionResult> UpdateUsername(ChangeUsernameDto changeUsernameDto)
    {
        var result = await _userSettingsService.ChangeUsername(changeUsernameDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("UpdateEmail")]
    public async Task<IActionResult> UpdateEmail(ChangeEmailDto changeEmailDto)
    {
        var result = await _userSettingsService.ChangeEmail(changeEmailDto);
        return StatusCode(result.StatusCode, result);
    }
}