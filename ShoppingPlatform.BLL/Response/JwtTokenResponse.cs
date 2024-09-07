namespace ShoppingPlatform.BLL.Response;

public class JwtTokenResponse
{
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }        
}