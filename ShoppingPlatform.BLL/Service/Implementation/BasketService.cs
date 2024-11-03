using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;
using ShoppingPlatform.BLL.Dto.BasketDto;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class BasketService : IBasketService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BasketService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<BasketGetDto>> GetUserBasket()
    {
        Log.Logger.Information(nameof(GetUserBasket) + "GetUserBasket called");
        var response = new ApiResponse<BasketGetDto>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(AddProductToBasket) + "User not logged in: {userEmail}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not logged in", 404);
                Log.Logger.Warning(nameof(AddProductToBasket) + "User not logged in with this email: {email}",
                    userEmail);
                return response;
            }

            var basket = await _unitOfWork.BasketRepository.GetUserBasket(user.Id);

            if (basket == null)
            {
                response.Failure("Basket not found", 404);
                Log.Logger.Warning(nameof(GetUserBasket) + "Basket not found for user: {userId}", user.Id);
                return response;
            }

            var basketDto = new BasketGetDto
            {
                UserId = basket.UserId,
                BasketItems = basket.BasketItems.Select(b => new BasketItemGetDto
                {
                    ProductId = b.ProductId,
                    ProductName = b.Product.Name,
                    ProductPrice = b.Product.Price,
                    Quantity = b.Quantity
                }).ToList()
            };

            response.Success(basketDto, 200);
            Log.Logger.Information(nameof(GetUserBasket) + "Basket retrieved successfully for user: {userId}", user.Id);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetUserBasket) + "Error happened while getting user's basket: {message}",
                e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<AddProductToBasketDto>> AddProductToBasket(
        AddProductToBasketDto addProductToBasketDto)
    {
        Log.Logger.Information(nameof(AddProductToBasket) + "Adding product to basket for this product: {id} started",
            addProductToBasketDto.ProductId);
        var response = new ApiResponse<AddProductToBasketDto>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(AddProductToBasket) + "User not logged in: {userEmail}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not logged in", 404);
                Log.Logger.Warning(nameof(AddProductToBasket) + "User not logged in with this email: {email}",
                    userEmail);
                return response;
            }

            // Getting user's basket
            var basket = await _unitOfWork.BasketRepository.GetUserBasket(user.Id);

            if (basket == null)
            {
                basket = new Basket
                {
                    UserId = user.Id,
                    CreationDate = DateTime.Now,
                    TotalPrice = 0,
                    BasketItems = new List<BasketItem>()
                };
                await _unitOfWork.BasketRepository.Create(basket);
                await _unitOfWork.Commit();
            }

            var product = await _unitOfWork.ProductRepository.GetProductById(addProductToBasketDto.ProductId);
            
            if (product == null)
            {
                response.Failure("Product not found", 404);
                Log.Logger.Warning(nameof(AddProductToBasket) + "Product not found with this id: {id}",
                    addProductToBasketDto.ProductId);
                return response;
            }

            var basketProduct = basket.BasketItems
                .FirstOrDefault(bp => bp.ProductId == addProductToBasketDto.ProductId);

            if (basketProduct != null)
            {
                basketProduct.Quantity += addProductToBasketDto.Quantity;
            }
            else
            {
                basket.BasketItems.Add(new BasketItem()
                {
                    ProductId = addProductToBasketDto.ProductId,
                    Quantity = addProductToBasketDto.Quantity
                });
            }

            basket.TotalPrice = 0;
            foreach (var item in basket.BasketItems)
            {
                await _unitOfWork.ProductRepository.GetProductById(item.ProductId);
                
                basket.TotalPrice += item.Quantity * product.Price;
            }

            var result = await _unitOfWork.Commit();

            if (result > 0)
            {
                response.Success(addProductToBasketDto, 201);
            }

            Log.Logger.Information(nameof(AddProductToBasket) + "Product added successfully with this id: {id}",
                addProductToBasketDto.ProductId);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(AddProductToBasket) + "Error happened while adding product: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<decimal>> GetTotalPrice()
    {
        Log.Logger.Information(nameof(GetTotalPrice) + "GetTotalPrice called");
        var response = new ApiResponse<decimal>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(GetTotalPrice) + "User not logged in: {userEmail}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(GetTotalPrice) + "User not found with this email: {email}", userEmail);
                return response;
            }

            var basket = await _unitOfWork.BasketRepository.GetUserBasket(user.Id);

            if (basket == null)
            {
                response.Success(0, 200);
                Log.Logger.Information(nameof(GetTotalPrice) + "Basket not found for user: {userId}", user.Id);
                return response;
            }

            var totalPrice = basket.TotalPrice;

            response.Success(totalPrice, 200);
            Log.Logger.Information(
                nameof(GetTotalPrice) + "Total price retrieved for user: {userId}, total: {totalPrice}", user.Id,
                totalPrice);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetTotalPrice) + "Error happened while retrieving total price: {Message}",
                e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<bool>> DeleteProductFromBasket(int productId)
    {
        Log.Logger.Information(nameof(DeleteProductFromBasket) + "Removing product from basket: {productId} started",
            productId);
        var response = new ApiResponse<bool>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(DeleteProductFromBasket) + "User not logged in: {userEmail}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(DeleteProductFromBasket) + "User not found with this email: {email}",
                    userEmail);
                return response;
            }

            var basket = await _unitOfWork.BasketRepository.GetUserBasket(user.Id);

            if (basket == null)
            {
                response.Failure("Basket not found", 404);
                Log.Logger.Warning(nameof(DeleteProductFromBasket) + "Basket not found for user: {userId}", user.Id);
                return response;
            }

            // Remove all instances of the product
            var basketItemsToRemove = basket.BasketItems
                .Where(bp => bp.ProductId == productId)
                .ToList();

            if (!basketItemsToRemove.Any())
            {
                response.Failure("Product not found in basket", 404);
                Log.Logger.Warning(nameof(DeleteProductFromBasket) + "Product not found in basket with id: {productId}",
                    productId);
                return response;
            }

            foreach (var item in basketItemsToRemove)
            {
                basket.BasketItems.Remove(item);
            }

            // Recalculate total price
            basket.TotalPrice = 0;
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.ProductRepository.GetProductById(item.ProductId);
                if (product != null)
                {
                    basket.TotalPrice += item.Quantity * product.Price;
                }
            }

            var result = await _unitOfWork.Commit();

            if (result > 0)
            {
                response.Success(true, 200);
                Log.Logger.Information(
                    nameof(DeleteProductFromBasket) +
                    "All instances of product removed successfully from basket: {productId}", productId);
            }
            else
            {
                response.Failure("Failed to remove product from basket", 500);
                Log.Logger.Error(nameof(DeleteProductFromBasket) + "Failed to remove product from basket: {productId}",
                    productId);
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(
                nameof(DeleteProductFromBasket) + "Error happened while removing product from basket: {Message}",
                e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<bool>> RemoveOneQuantityFromBasket(int productId)
    {
        Log.Logger.Information(
            nameof(RemoveOneQuantityFromBasket) + "Removing one quantity of product from basket: {productId} started",
            productId);
        var response = new ApiResponse<bool>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(RemoveOneQuantityFromBasket) + "User not logged in: {userEmail}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(RemoveOneQuantityFromBasket) + "User not found with this email: {email}", userEmail);
                return response;
            }
            
            var basket = await _unitOfWork.BasketRepository.GetUserBasket(user.Id);

            if (basket == null)
            {
                response.Failure("Basket not found", 404);
                Log.Logger.Warning(nameof(RemoveOneQuantityFromBasket) + "Basket not found for user: {userId}", user.Id);
                return response;
            }

            // Find the basket item
            var basketProduct = basket.BasketItems
                .FirstOrDefault(bp => bp.ProductId == productId);

            if (basketProduct == null)
            {
                response.Failure("Product not found in basket", 404);
                Log.Logger.Warning(
                    nameof(RemoveOneQuantityFromBasket) + "Product not found in basket with id: {productId}", productId);
                return response;
            }

            // Remove one quantity
            basketProduct.Quantity -= 1;

            if (basketProduct.Quantity <= 0)
            {
                basket.BasketItems.Remove(basketProduct);
            }

            // Recalculate total price
            basket.TotalPrice = 0;
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.ProductRepository.GetProductById(item.ProductId);
                if (product != null)
                {
                    basket.TotalPrice += item.Quantity * product.Price;
                }
            }

            var result = await _unitOfWork.Commit();

            if (result > 0)
            {
                response.Success(true, 200);
                Log.Logger.Information(nameof(RemoveOneQuantityFromBasket) + "One quantity of product removed successfully from basket: {productId}", productId);
            }
            else
            {
                response.Failure("Failed to remove product quantity from basket", 500);
                Log.Logger.Error(nameof(RemoveOneQuantityFromBasket) + "Failed to remove product quantity from basket: {productId}", productId);
                return response;
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(RemoveOneQuantityFromBasket) + "Error happened while removing one quantity of product from basket: {Message}", e.Message);
            throw;
        }

        return response;
    }
}