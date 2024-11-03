using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;

    public ReviewService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }
    
    public async Task<ApiResponse<bool>> AddReview(int productId, string content)
    {
        Log.Logger.Information(nameof(AddReview) + "AddReview called");
        var response = new ApiResponse<bool>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(AddReview) + "User not logged in: {userEmail}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(AddReview) + "User not found with this email: {email}", userEmail);
                return response;
            }

            var product = await _unitOfWork.ProductRepository.GetProductById(productId);
            
            if (product == null)
            {
                response.Failure("Product not found", 404);
                Log.Logger.Warning(nameof(AddReview) + "Product not found with this id: {id}", productId);
                return response;
            }

            var review = new Review
            {
                Content = content,
                ProductId = productId,
                UserId = user.Id
            };

            await _unitOfWork.ReviewRepository.Create(review);
            var result = await _unitOfWork.Commit();

            if (result > 0)
            {
                response.Success(true, 201);
            }
            
            Log.Logger.Information(nameof(AddReview) + "Adding review completed succesfully");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(AddReview) + "Error happened while adding review: {message}", e.Message);
            throw;
        }

        return response;
    }
}