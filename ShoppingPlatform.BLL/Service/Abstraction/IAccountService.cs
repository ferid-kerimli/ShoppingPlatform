using Microsoft.Extensions.Configuration;
using ShoppingPlatform.BLL.Dto.AccountDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IAccountService    
{
    Task<ApiResponse<JwtTokenResponse>> Login(LoginDto loginDto, IConfiguration configuration);
    Task<ApiResponse<RegisterDto>> Register(RegisterDto registerDto);
    // Task<ApiResponse<bool>> Verify(VerifyDto verifyDto);
    // Task<ApiResponse<bool>> ResendVerificationCode(string email);
    Task<ApiResponse<bool>> Logout();   
}