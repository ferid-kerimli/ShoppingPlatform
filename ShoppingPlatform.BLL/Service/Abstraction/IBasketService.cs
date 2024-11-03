using ShoppingPlatform.BLL.Dto.BasketDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IBasketService
{
    Task<ApiResponse<BasketGetDto>> GetUserBasket();
    Task<ApiResponse<AddProductToBasketDto>> AddProductToBasket(AddProductToBasketDto addProductToBasketDto);
    Task<ApiResponse<decimal>> GetTotalPrice();
    Task<ApiResponse<bool>> DeleteProductFromBasket(int productId);
    Task<ApiResponse<bool>> RemoveOneQuantityFromBasket(int productId);
}