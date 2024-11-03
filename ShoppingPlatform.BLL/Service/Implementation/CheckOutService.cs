using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;
using ShoppingPlatform.BLL.Dto.CheckOutDto;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class CheckOutService : ICheckOutService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CheckOutService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<ApiResponse<PaymentDto>> ProcessPayment(PaymentDto paymentDto)
    {
        Log.Logger.Information(nameof(ProcessPayment) + "ProcessPayment called");
        var response = new ApiResponse<PaymentDto>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(ProcessPayment) + "User not logged in: {email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(ProcessPayment) + "User not found");
                return response;
            }

            var basket = await _unitOfWork.BasketRepository.GetUserBasket(user.Id);
            
            if (basket == null)
            {
                response.Failure("Basket not found", 404);
                Log.Logger.Warning(nameof(ProcessPayment) + "Basket not found for user: {userId}", user.Id);
                return response;
            }

            if (paymentDto.Amount != basket.TotalPrice)
            {
                response.Failure("Payment amount does not match basket total", 400);
                Log.Logger.Warning(nameof(ProcessPayment) + "Payment amount mismatch. Expected: {expected}, Provided: {provided}",
                    basket.TotalPrice, paymentDto.Amount);
                return response;
            }
            
            basket.BasketItems.Clear();
            basket.TotalPrice = 0;

            _unitOfWork.BasketRepository.Update(basket);
            await _unitOfWork.Commit();

            var check = GenerateCheck(user, paymentDto.Amount);
            paymentDto.Check = check;
            
            response.Success(paymentDto, 200);
            Log.Logger.Information(nameof(ProcessPayment) + "Payment successful for user: {email}", userEmail);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(ProcessPayment) + "Error happened while processing payment: {message}", e.Message);
            throw;
        }

        return response;
    }

    private static string GenerateCheck(AppUser user, decimal amount)
    {
        var check = new StringBuilder();
        
        check.AppendLine("*********************************");
        check.AppendLine("          PAYMENT RECEIPT        ");
        check.AppendLine("*********************************");
        check.AppendLine($"User: {user.UserName}");
        check.AppendLine($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        check.AppendLine($"Amount Paid: {amount:C}"); 
        check.AppendLine("*********************************");
        check.AppendLine("     Thank you for shopping!     ");
        check.AppendLine("*********************************");

        return check.ToString();
    }
}