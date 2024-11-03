using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShoppingPlatform.BLL.Dto.JwtDto;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using Serilog;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class TokenService : ITokenService
{
    public async Task<JwtTokenResponse> GenerateToken(TokenRequest tokenRequest, IConfiguration configuration, IList<string> roles)
    {
        Log.Logger.Information(nameof(GenerateToken) + "Generating token for {Email}", tokenRequest.Email);
        try
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"] ?? throw new InvalidOperationException()));
            
            var dateTimeNow = DateTime.Now;
            var expireMinute = int.Parse(configuration["Jwt:Expire"] ?? throw new InvalidOperationException());
        
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, tokenRequest.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            var jwt = new JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssuer"],
                audience: configuration["Jwt:ValidAudience"],
                claims: claims,
                notBefore: dateTimeNow,
                expires: dateTimeNow.AddMinutes(expireMinute),
                signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            );
            
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var expireDate = dateTimeNow.AddMinutes(expireMinute);
    
            Log.Logger.Information(nameof(GenerateToken) + "Token generated successfully for {Email}", tokenRequest.Email);
    
            return await Task.FromResult(new JwtTokenResponse()
            {
                Token = token,
                ExpireDate = expireDate
            });
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, nameof(GenerateToken) + "Error occurred while generating token for {Email}", tokenRequest.Email);
            throw;
        }
    }
}
