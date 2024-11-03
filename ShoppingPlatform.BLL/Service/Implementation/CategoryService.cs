using AutoMapper;
using Serilog;
using ShoppingPlatform.BLL.Dto.CategoryDto;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public Task<ApiResponse<List<CategoryGetDto>>> GetAllCategories()
    {
        Log.Logger.Information(nameof(GetAllCategories) + "GetAllCategories called");
        var response = new ApiResponse<List<CategoryGetDto>>();

        try
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();

            var mappedCategories = _mapper.Map<List<CategoryGetDto>>(categories);
            
            response.Success(mappedCategories, 200);
            Log.Logger.Information(nameof(GetAllCategories) + "GetAllCategories completed successfully");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetAllCategories) + "Error happened while getting all categories: {Message}", e.Message);
            throw;
        }

        return Task.FromResult(response);
    }

    public async Task<ApiResponse<CategoryGetDto>> GetCategoryById(int id)
    {
        Log.Logger.Information(nameof(GetCategoryById) + "GetCategoryById called for {id}", id);
        var response = new ApiResponse<CategoryGetDto>();

        try
        {
            var category = await _unitOfWork.CategoryRepository.GetById(id);

            if (category == null)
            {
                response.Failure("Category not found", 404);
                Log.Logger.Warning(nameof(GetCategoryById) + "Category not found with this id: {id}", id);
                return response;
            }
            
            var mappedCategory = _mapper.Map<CategoryGetDto>(category);
            
            response.Success(mappedCategory, 200);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetCategoryById) + "Error happened while getting product: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<CategoryCreateDto>> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        Log.Logger.Information(nameof(CreateCategory) + "CreateCategory called");
        var response = new ApiResponse<CategoryCreateDto>();

        try
        {
            var mappedCategory = _mapper.Map<Category>(categoryCreateDto);
            await _unitOfWork.CategoryRepository.Create(mappedCategory);

            var result = await _unitOfWork.Commit();
            
            if (result > 0)
            {
                response.Success(categoryCreateDto, 201);
            }
            
            Log.Logger.Information(nameof(CreateCategory) + "CreateCategory successfully finished");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(CreateCategory) + "Error happened while creating product: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<CategoryUpdateDto>> UpdateCategory(CategoryUpdateDto categoryUpdateDto)
    {
        Log.Logger.Information(nameof(UpdateCategory) + "UpdateCategory called");
        var response = new ApiResponse<CategoryUpdateDto>();

        try
        {
            var category = await _unitOfWork.CategoryRepository.GetById(categoryUpdateDto.Id);

            if (category == null)
            {
                response.Failure("Category not found", 404);
                Log.Logger.Warning(nameof(UpdateCategory) + "Category not found with this id: {id}", categoryUpdateDto.Id);
                return response;
            }

            var mappedCategory = _mapper.Map(categoryUpdateDto, category);
            _unitOfWork.CategoryRepository.Update(mappedCategory);

            var result = await _unitOfWork.Commit();

            if (result > 0)
            {
                response.Success(categoryUpdateDto, 200);
            }
            
            Log.Logger.Information(nameof(UpdateCategory) + "UpdateCategory successfully completed for this id: {id}", categoryUpdateDto.Id);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(UpdateCategory) + "Error happened while updating category: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<bool>> DeleteCategory(int id)
    {
        Log.Logger.Information(nameof(DeleteCategory) + "DeleteCategory called");
        var response = new ApiResponse<bool>();

        try
        {
            var category = await _unitOfWork.CategoryRepository.GetById(id);

            if (category == null)
            {
                response.Failure("Category not found", 404);
                Log.Logger.Warning(nameof(DeleteCategory) + "Category not found with this id: {id}", id);
                return response;
            }

            _unitOfWork.CategoryRepository.Delete(category);
            
            response.Success(true, 200);
            Log.Logger.Information(nameof(DeleteCategory) + "DeleteCategory successfully completed for this id: {id}", id);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(DeleteCategory) + "Error happened while deleting category: {Message}", e.Message);
            throw;
        }

        return response;
    }
}