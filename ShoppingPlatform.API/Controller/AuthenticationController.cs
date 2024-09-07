using Microsoft.AspNetCore.Mvc;
using ShoppingPlatform.BLL.Dto.AccountDto;
using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AuthenticationController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto, IConfiguration configuration)
    {
        var result = await _accountService.Login(loginDto, configuration);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _accountService.Register(registerDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await _accountService.Logout();
        return StatusCode(result.StatusCode, result);
    }

    // [HttpPost("Verify")]
    // public async Task<IActionResult> Verify(VerifyDto verifyDto)
    // {
    //     var result = await _accountService.Verify(verifyDto);
    //     return StatusCode(result.StatusCode, result);
    // }
    //
    // [HttpPost("ResendVerificationCode")]
    // public async Task<IActionResult> ResendVerificationCode(string email)
    // {
    //     var result = await _accountService.ResendVerificationCode(email);
    //     return StatusCode(result.StatusCode, result);
    // }
}