using Microsoft.Extensions.Configuration;
using ShoppingPlatform.BLL.Dto.JwtDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface ITokenService
{
    Task<JwtTokenResponse> GenerateToken(TokenRequest tokenRequest, IConfiguration configuration, IList<string> roles);
}