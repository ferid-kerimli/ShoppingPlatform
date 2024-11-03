using AutoMapper;
using Serilog;
using ShoppingPlatform.BLL.Dto.ProductDto;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Abstraction;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class AllProductsService : IAllProductsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AllProductsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ProductGetDto>>> GetAllProducts()
    {
        Log.Logger.Information(nameof(GetAllProducts) + "GetAllProducts called");
        var response = new ApiResponse<List<ProductGetDto>>();

        try
        {
            var products = await _unitOfWork.ProductRepository.GetAllProducts();

            if (products.Count == 0)
            {
                response.Failure("Products not found", 404);
                Log.Logger.Warning(nameof(GetAllProducts) + "Products not found");
                return response;
            }

            var productGetDtoS = products.Select(product =>
            {
                var productDto = _mapper.Map<ProductGetDto>(product);
                
                productDto.Files = product.ProductImages.Select(pi => pi.FilePath).ToList();
                
                return productDto;
            }).ToList();
            
            response.Success(productGetDtoS, 200);
            Log.Logger.Information(nameof(GetAllProducts) + " Products retrieved successfully");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetAllProducts) + "Error happened while getting all products: {message}", e.Message);
            throw;
        }

        return response;
    }
    
    public async Task<ApiResponse<ProductGetDto>> GetProductById(int id)
    {
        Log.Logger.Information(nameof(GetProductById) + " GetProductById called");
        var response = new ApiResponse<ProductGetDto>();

        try
        {
            var product = await _unitOfWork.ProductRepository.GetProductById(id);

            if (product == null)
            {
                response.Failure("Product not found", 404);
                Log.Logger.Warning(nameof(GetProductById) + " Product not found with id: {id}", id);
                return response;
            }

            var productGetDto = _mapper.Map<ProductGetDto>(product);
            productGetDto.Files = product.ProductImages.Select(pi => pi.FilePath).ToList();

            response.Success(productGetDto, 200);
            Log.Logger.Information(nameof(GetProductById) + " Product retrieved successfully for id: {id}", id);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetProductById) + " Error happened while getting product by id: {message}", e.Message);
            throw;
        }

        return response;
    }


    public async Task<ApiResponse<List<ProductGetDto>>> GetProductsByCategoryId(int categoryId)
    {
        Log.Logger.Information(nameof(GetProductsByCategoryId) + " GetProductByCategoryId called");
        var response = new ApiResponse<List<ProductGetDto>>();

        try
        {
            var products = await _unitOfWork.ProductRepository.GetAllProductsByCategoryId(categoryId);

            if (products.Count == 0)
            {
                response.Failure("No products found for this category", 404);
                Log.Logger.Warning(nameof(GetProductsByCategoryId) + " No products found for category id: {categoryId}", categoryId);
                return response;
            }
            
            var productGetDtoS = products.Select(product => {
                var productDto = _mapper.Map<ProductGetDto>(product);
                
                productDto.Files = product.ProductImages.Select(pi => pi.FilePath).ToList();
                
                return productDto;
            }).ToList();

            response.Success(productGetDtoS, 200);
            Log.Logger.Information(nameof(GetProductsByCategoryId) + " Products retrieved successfully for category id: {categoryId}", categoryId);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetProductsByCategoryId) + " Error happened while getting products by category id: {message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<ProductRatingGetDto>> GetProductRating(int productId)
    {
        Log.Logger.Information(nameof(GetProductRating) + "Getting average rating for product: {productId} started", productId);
        var response = new ApiResponse<ProductRatingGetDto>();

        try
        {
            var averageRating = await _unitOfWork.RatingRepository.GetAverageRating(productId);

            if (averageRating == 0)
            {
                response.Failure("No rating found", 404);
                Log.Logger.Warning(nameof(GetProductRating) + "No rating found");
                return response;
            }

            var ratingDto = new ProductRatingGetDto
            {
                AverageRating = averageRating
            };

            response.Success(ratingDto, 200);
            Log.Logger.Information(nameof(GetProductRating) + "Average rating for product {productId}: {averageRating}", productId, averageRating);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetProductRating) + "Error occurred while getting average rating for product {productId}: {message}", productId, e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<List<ProductReviewGetDto>>> GetProductReviews(int productId)
    {
        Log.Logger.Information(nameof(GetProductReviews) + "GetProductReviews called");
        var response = new ApiResponse<List<ProductReviewGetDto>>();

        try
        {
            var reviews = await _unitOfWork.ReviewRepository.GetReviewsByProductId(productId);
            
            if (reviews.Count == 0)
            {
                response.Failure("No reviews found", 404);
                Log.Logger.Warning(nameof(GetProductReviews) + "No reviews found");
                return response;
            }
            
            var reviewDtoS = reviews.Select(r => new ProductReviewGetDto
            {
                Content = r.Content,
                UserId = r.UserId
            }).ToList();
            
            response.Success(reviewDtoS, 200);
            Log.Logger.Information(nameof(GetProductReviews) + "GetProductReviews completed successfully");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetProductReviews) + "Error happened while getting reviews: {message}", e.Message);
            throw;
        }

        return response;
    }
}