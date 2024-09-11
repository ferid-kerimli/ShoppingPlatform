using ShoppingPlatform.BLL.Dto.CheckOutDto;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Service.Abstraction;

public interface ICheckOutService
{
    Task<ApiResponse<PaymentDto>> ProcessPayment(PaymentDto paymentDto);
}