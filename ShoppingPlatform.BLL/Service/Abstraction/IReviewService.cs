using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IReviewService
{
    Task<ApiResponse<bool>> AddReview(int productId, string content);
}