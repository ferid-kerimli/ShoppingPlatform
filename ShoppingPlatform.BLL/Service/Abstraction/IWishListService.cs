using ShoppingPlatform.BLL.Dto.WishListDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface IWishListService
{
    Task<ApiResponse<WishListGetDto>> GetUserWishlist();
    Task<ApiResponse<AddProductToWishListDto>> AddProductToWishList(AddProductToWishListDto addProductToWishListDto);
    Task<ApiResponse<bool>> RemoveProductFromWishlist(int productId);
}