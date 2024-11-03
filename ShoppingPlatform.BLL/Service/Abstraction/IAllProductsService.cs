using ShoppingPlatform.BLL.Dto.ProductDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IAllProductsService
{
    Task<ApiResponse<List<ProductGetDto>>> GetAllProducts();
    Task<ApiResponse<ProductGetDto>> GetProductById(int id);
    Task<ApiResponse<List<ProductGetDto>>> GetProductsByCategoryId(int categoryId);
    Task<ApiResponse<ProductRatingGetDto>> GetProductRating(int productId);
    Task<ApiResponse<List<ProductReviewGetDto>>> GetProductReviews(int productId);
}