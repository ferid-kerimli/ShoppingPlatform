using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingPlatform.BLL.Dto.WishListDto;
using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.API.Controller;

[Route("api")]
[ApiController]
[Authorize]
public class WishlistController : ControllerBase
{
    private readonly IWishListService _wishListService;

    public WishlistController(IWishListService wishListService)
    {
        _wishListService = wishListService;
    }

    [HttpGet("MyWishlist")]
    public async Task<IActionResult> GetUserWishlist()
    {
        var result = await _wishListService.GetUserWishlist();
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("AddProductToMyWishlist")]
    public async Task<IActionResult> AddProductToWishlist(AddProductToWishListDto addProductToWishListDto)
    {
        var result = await _wishListService.AddProductToWishList(addProductToWishListDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("RemoveProductFromMyWishlist")]
    public async Task<IActionResult> RemoveProductFromWishlist(int productId)
    {
        var result = await _wishListService.RemoveProductFromWishlist(productId);
        return StatusCode(result.StatusCode, result);
    }
}