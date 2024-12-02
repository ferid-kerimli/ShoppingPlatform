using ShoppingPlatform.BLL.Dto.ProductDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IRatingService
{
    Task<ApiResponse<bool>> AddRating(int productId, int value);
    Task<ApiResponse<List<ProductGetDto>>> GetProductsWithDescendingRating();
    Task<ApiResponse<List<ProductGetDto>>> GetTop10Products(int topCount);
}