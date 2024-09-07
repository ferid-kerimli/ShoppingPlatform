using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IRatingService
{
    Task<ApiResponse<bool>> AddRating(int productId, int value);
}