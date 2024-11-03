namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IVerificationService
{
    Task StoreVerificationCode(string email, string code);
    Task<string> GetStoredVerificationCode(string email);
    Task<DateTime?> GetCodeTimestamp(string email);
    Task<bool> IsCodeValid(string email, string code);
    Task<bool> CanResendCode(string email);
    Task ResendVerificationCode(string email);
}