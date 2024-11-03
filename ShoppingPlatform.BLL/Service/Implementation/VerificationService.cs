using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class VerificationService : IVerificationService
{
    private static readonly Dictionary<string, (string Code, DateTime Timestamp)> VerificationData = new Dictionary<string, (string, DateTime)>();
    private static readonly object LockObject = new object();
    private const int CodeExpiryMinutes = 2; 
    
    public async Task StoreVerificationCode(string email, string code)
    {
        lock (LockObject)
        {
            VerificationData[email] = (code, DateTime.UtcNow);
        }

        await Task.CompletedTask;
    }

    public Task<string> GetStoredVerificationCode(string email)
    {
        lock (LockObject)
        {
            VerificationData.TryGetValue(email, out var data);
            return Task.FromResult(data.Code);
        }
    }

    public Task<DateTime?> GetCodeTimestamp(string email)
    {
        lock (LockObject)
        {
            VerificationData.TryGetValue(email, out var data);
            return Task.FromResult<DateTime?>(data.Timestamp);
        }
    }

    public async Task<bool> IsCodeValid(string email, string code)
    {
        var storedCode = await GetStoredVerificationCode(email);
        var timestamp = await GetCodeTimestamp(email);

        if (storedCode == code && timestamp.HasValue)
        {
            var isValid = DateTime.UtcNow - timestamp.Value <= TimeSpan.FromMinutes(CodeExpiryMinutes);
            return isValid;
        }
        return false;
    }

    public async Task<bool> CanResendCode(string email)
    {
        var timestamp = await GetCodeTimestamp(email);
        if (timestamp.HasValue)
        {
            var elapsedTime = DateTime.UtcNow - timestamp.Value;
            return elapsedTime >= TimeSpan.FromMinutes(CodeExpiryMinutes);
        }
        return true;
    }

    public async Task ResendVerificationCode(string email)
    {
        var newCode = new Random().Next(100000, 999999).ToString();
        await StoreVerificationCode(email, newCode);
    }
}