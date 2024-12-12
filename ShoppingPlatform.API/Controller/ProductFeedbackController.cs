using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.API.Controller;

[Route("api")]
[ApiController]
[Authorize]
public class ProductFeedbackController : ControllerBase
{
    private readonly IRatingService _ratingService;
    private readonly IReviewService _reviewService;

    public ProductFeedbackController(IRatingService ratingService, IReviewService reviewService)
    {
        _ratingService = ratingService;
        _reviewService = reviewService;
    }
    
    [HttpPost("AddRatingToProduct")]
    public async Task<IActionResult> AddRatingToProduct(int productId, int value)
    {
        var result = await _ratingService.AddRating(productId, value);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("AddReviewToProduct")]
    public async Task<IActionResult> AddReviewToProduct(int productId, string content)
    {
        var result = await _reviewService.AddReview(productId, content);
        return StatusCode(result.StatusCode, result);
    }
}