using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingPlatform.BLL.Dto.ProductDto;
using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.API.Controller;

[Route("api")]
[ApiController]
[Authorize]
public class MyProductsController : ControllerBase
{
    private readonly IMyProductsService _myProductsService;

    public MyProductsController(IMyProductsService myProductsService)
    {
        _myProductsService = myProductsService;
    }

    [HttpGet("GetMyProducts")]
    public async Task<IActionResult> GetMyAllProducts()
    {
        var result = await _myProductsService.GetMyAllProducts();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("MyProduct/{id}")]
    public async Task<IActionResult> GetMyProductById(int id)
    {
        var result = await _myProductsService.GetMyProductById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("MyProduct/category/{categoryId}")]
    public async Task<IActionResult> GetMyProductsByCategoryId(int categoryId)
    {
        var result = await _myProductsService.GetMyProductsByCategoryId(categoryId);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto productCreateDto)
    {
        var result = await _myProductsService.CreateProduct(productCreateDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("UpdateProduct")]
    public async Task<IActionResult> UpdateProduct(ProductUpdateDto productUpdateDto)
    {
        var result = await _myProductsService.UpdateProduct(productUpdateDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("DeleteProduct")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _myProductsService.DeleteProduct(id);
        return StatusCode(result.StatusCode, result);
    }
}