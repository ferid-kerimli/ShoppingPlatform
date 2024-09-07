using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class RatingService : IRatingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;

    public RatingService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<ApiResponse<bool>> AddRating(int productId, int value)
    {
        Log.Logger.Information(nameof(AddRating) + "AddRating called");
        var response = new ApiResponse<bool>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(AddRating) + "User not logged in: {userEmail}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(AddRating) + "User not found with this email: {email}", userEmail);
                return response;
            }

            var product = await _unitOfWork.ProductRepository.GetProductById(productId);
            
            if (product == null)
            {
                response.Failure("Product not found", 404);
                Log.Logger.Warning(nameof(AddRating) + "Product not found with this id: {id}", productId);
                return response;
            }

            var rating = new Rating
            {
                UserId = user.Id,
                ProductId = productId,
                Value = value
            };
            
            await _unitOfWork.RatingRepository.Create(rating);
            await _unitOfWork.Commit();
            
            var averageRating = await _unitOfWork.RatingRepository.GetAverageRating(productId);
            product.AverageRating = averageRating;
            
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Commit();
            
            response.Success(true, 201);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(AddRating) + "Error happened while adding rating: {message}", e.Message);
            throw;
        }

        return response;
    }
}