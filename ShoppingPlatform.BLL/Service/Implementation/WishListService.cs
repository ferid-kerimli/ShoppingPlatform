using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;
using ShoppingPlatform.BLL.Dto.WishListDto;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class WishListService : IWishListService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WishListService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<WishListGetDto>> GetUserWishlist()
    {
        Log.Logger.Information(nameof(GetUserWishlist) + "GetUserWishList called");
        var response = new ApiResponse<WishListGetDto>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(GetUserWishlist) + "User not logged in with this email: {email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(GetUserWishlist) + "User not found with this email: {email}", userEmail);
                return response;
            }

            var wishlist = await _unitOfWork.WishlistRepository.GetUserWishlist(user.Id);

            if (wishlist == null)
            {
                response.Failure("Wishlist not found", 404);
                Log.Logger.Warning(nameof(GetUserWishlist) + "Wishlist not found for user: {userId}", user.Id);
                return response;
            }
            
            var wishListDto = new WishListGetDto
            {
                UserId = wishlist.UserId,
                WishlistItems = wishlist.WishlistItems.Select(wi => new WishListItemGetDto
                {
                    ProductId = wi.ProductId,
                    ProductName = wi.Product.Name,
                    ProductPrice = wi.Product.Price
                }).ToList()
            };

            response.Success(wishListDto, 200); 
            Log.Logger.Information(nameof(GetUserWishlist) + "Wishlist retrieved successfully for user: {userId}", user.Id);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetUserWishlist) + "Error happened while getting user's wishlist: {message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<AddProductToWishListDto>> AddProductToWishList(AddProductToWishListDto addProductToWishListDto)
    {
        Log.Logger.Information(nameof(AddProductToWishList) + "AddProductToWishList called");
        var response = new ApiResponse<AddProductToWishListDto>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(AddProductToWishList) + "User not logged in with this email: {email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(AddProductToWishList) + "User not found with this email: {email}", userEmail);
                return response;
            }
            
            // Get user's wishlist
            var wishlist = await _unitOfWork.WishlistRepository.GetUserWishlist(user.Id);

            if (wishlist == null)
            {
                wishlist = new WishList { UserId = user.Id, WishlistItems = new List<WishlistItem>()};
                await _unitOfWork.WishlistRepository.Create(wishlist);
                await _unitOfWork.Commit();
            }

            var wishListProduct = wishlist.WishlistItems
                .FirstOrDefault(wp => wp.ProductId == addProductToWishListDto.ProductId);

            if (wishListProduct != null)
            {
                response.Failure("Product is already in the wishlist", 409); // 409 Conflict
                Log.Logger.Warning(nameof(AddProductToWishList) + "Product already in wishlist: {productId}", addProductToWishListDto.ProductId);
                return response;
            }
            
            wishlist.WishlistItems.Add(new WishlistItem
            {
                ProductId = addProductToWishListDto.ProductId,
                WishListId = wishlist.Id
            });
            
            await _unitOfWork.Commit();

            response.Success(addProductToWishListDto, 201);
            Log.Logger.Information(nameof(AddProductToWishList) + "Product added to wishlist successfully: {productId}", addProductToWishListDto.ProductId);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(AddProductToWishList) + "Error happened while adding product to wishlist: {Message}", e.Message);
            throw;
        }

        return response;
    }

public async Task<ApiResponse<bool>> RemoveProductFromWishlist(int productId)
{
    Log.Logger.Information(nameof(RemoveProductFromWishlist) + "Removing product from wishlist: {productId} started", productId);
    var response = new ApiResponse<bool>();

    try
    {
        var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

        if (userEmail == null)
        {
            response.Failure("User not logged in", 401);
            Log.Logger.Warning(nameof(RemoveProductFromWishlist) + "User not logged in: {userEmail}", userEmail);
            return response;
        }

        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user == null)
        {
            response.Failure("User not found", 404);
            Log.Logger.Warning(nameof(RemoveProductFromWishlist) + "User not found with this email: {email}", userEmail);
            return response;
        }

        var wishlist = await _unitOfWork.WishlistRepository.GetUserWishlist(user.Id);

        if (wishlist == null)
        {
            response.Failure("Wishlist not found", 404);
            Log.Logger.Warning(nameof(RemoveProductFromWishlist) + "Wishlist not found for user: {userId}", user.Id);
            return response;
        }

        // Find and remove the product from the wishlist
        var itemToRemove = wishlist.WishlistItems
            .FirstOrDefault(wi => wi.ProductId == productId);

        if (itemToRemove == null)
        {
            response.Failure("Product not found in wishlist", 404);
            Log.Logger.Warning(nameof(RemoveProductFromWishlist) + "Product not found in wishlist with id: {productId}", productId);
            return response;
        }

        wishlist.WishlistItems.Remove(itemToRemove);

        var result = await _unitOfWork.Commit();

        if (result > 0)
        {
            response.Success(true, 200);
            Log.Logger.Information(nameof(RemoveProductFromWishlist) + "Product removed successfully from wishlist: {productId}", productId);
        }
        else
        {
            response.Failure("Failed to remove product from wishlist", 500);
            Log.Logger.Error(nameof(RemoveProductFromWishlist) + "Failed to remove product from wishlist: {productId}", productId);
        }
    }
    catch (Exception e)
    {
        response.Failure(e.Message, 500);
        Log.Logger.Error(nameof(RemoveProductFromWishlist) + "Error happened while removing product from wishlist: {Message}", e.Message);
        throw;
    }

    return response;
}

}