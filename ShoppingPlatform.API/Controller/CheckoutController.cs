using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingPlatform.BLL.Dto.CheckOutDto;
using ShoppingPlatform.BLL.Service.Abstraction;

namespace ShoppingPlatform.API.Controller;

[Route("api")]
[ApiController]
[Authorize]
public class CheckoutController : ControllerBase
{
    private readonly ICheckOutService _checkOutService;

    public CheckoutController(ICheckOutService checkOutService)
    {
        _checkOutService = checkOutService;
    }

    [HttpPost("ProcessPayment")]
    public async Task<IActionResult> ProcessPayment(PaymentDto paymentDto)
    {
        var result = await _checkOutService.ProcessPayment(paymentDto);
        return StatusCode(result.StatusCode, result);
    }
}