using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.API.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AllProductsController : ControllerBase
{
    private readonly IAllProductsService _allProductsService;
    private readonly IRatingService _ratingService;

    public AllProductsController(IAllProductsService allProductsService, IRatingService ratingService)
    {
        _allProductsService = allProductsService;
        _ratingService = ratingService;
    }

    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        var result = await _allProductsService.GetAllProducts();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetProductById")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var result = await _allProductsService.GetProductById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetProductsByCategoryId")]
    public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
    {
        var result = await _allProductsService.GetProductsByCategoryId(categoryId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetProductRating")]
    public async Task<IActionResult> GetProductRating(int productId)
    {
        var result = await _allProductsService.GetProductRating(productId);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpGet("GetProductReviews")]
    public async Task<IActionResult> GetProductReviews(int productId)
    {
        var result = await _allProductsService.GetProductReviews(productId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetProductsRatingInDescedingOrder")]
    public async Task<IActionResult> GetProductsRatingInDescendingOrder()
    {
        var result = await _ratingService.GetProductsWithDescendingRating();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetTop10Products")]
    public async Task<IActionResult> GetTop10Products()
    {
        var result = await _ratingService.GetTop10Products(10);
        return StatusCode(result.StatusCode, result);
    }
}