using ShoppingPlatform.BLL.Dto.CategoryDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface ICategoryService
{
    Task<ApiResponse<List<CategoryGetDto>>> GetAllCategories();
    Task<ApiResponse<CategoryGetDto>> GetCategoryById(int id);
    Task<ApiResponse<CategoryCreateDto>> CreateCategory(CategoryCreateDto categoryCreateDto);
    Task<ApiResponse<CategoryUpdateDto>> UpdateCategory(CategoryUpdateDto categoryUpdateDto);
    Task<ApiResponse<bool>> DeleteCategory(int id);
}