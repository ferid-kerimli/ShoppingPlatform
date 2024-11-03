using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingPlatform.BLL.Dto.BasketDto;
using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.API.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpPost("AddProductToBasket")]
    public async Task<IActionResult> AddProductToBasket([FromBody] AddProductToBasketDto addProductToBasketDto)
    {
        var result = await _basketService.AddProductToBasket(addProductToBasketDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetUserBasket")]
    public async Task<IActionResult> GetUserBasket()
    {
        var result = await _basketService.GetUserBasket();
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpGet("GetTotalPriceOfBasket")]
    public async Task<IActionResult> GetTotalPrice()
    {
        var result = await _basketService.GetTotalPrice();
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("DeleteProductFromBasket")]
    public async Task<IActionResult> DeleteProductFromBasket(int productId)
    {
        var result = await _basketService.DeleteProductFromBasket(productId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("RemoveOneQuantityFromBasket")]
    public async Task<IActionResult> RemoveOneQuantityFromBasket(int productId)
    {
        var result = await _basketService.RemoveOneQuantityFromBasket(productId);
        return StatusCode(result.StatusCode, result);
    }
}