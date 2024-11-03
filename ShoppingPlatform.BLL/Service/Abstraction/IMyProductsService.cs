using ShoppingPlatform.BLL.Dto.ProductDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IMyProductsService
{
    Task<ApiResponse<List<ProductGetDto>>> GetMyAllProducts();
    Task<ApiResponse<ProductGetDto>> GetMyProductById(int id);
    Task<ApiResponse<List<ProductGetDto>>> GetMyProductsByCategoryId(int categoryId);
    Task<ApiResponse<ProductCreateDto>> CreateProduct(ProductCreateDto productCreateDto);
    Task<ApiResponse<ProductUpdateDto>> UpdateProduct(ProductUpdateDto productUpdateDto);
    Task<ApiResponse<bool>> DeleteProduct(int id);
}