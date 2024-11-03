using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingPlatform.BLL.Dto.AccountDto;
using ShoppingPlatform.BLL.Mapper;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.BLL.Service.Implementation;

namespace ShoppingPlatform.BLL;

public static class BusinessLogicDependencyInjection
{
    public static void BusinessLogicInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MapperProfile));
        services.AddHttpContextAccessor();
        
        // var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();

        // services.AddFluentEmail(emailSettings.FromEmail)
        //     .AddSmtpSender(new SmtpClient(emailSettings.Host, emailSettings.Port)
        //     {
        //         Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password),
        //         EnableSsl = true
        //     });

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IMyProductsService, MyProductsService>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IWishListService, WishListService>();
        services.AddScoped<IUserSettingsService, UserSettingsService>();
        services.AddScoped<IAllProductsService, AllProductsService>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IVerificationService, VerificationService>();
        services.AddScoped<ICheckOutService, CheckOutService>();
    }
}