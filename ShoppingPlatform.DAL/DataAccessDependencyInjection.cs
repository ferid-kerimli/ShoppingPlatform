using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingPlatform.DAL.Abstraction;
using ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;
using ShoppingPlatform.DAL.Context;
using ShoppingPlatform.DAL.Implementation;
using ShoppingPlatform.DAL.Implementation.Repository.EntityRepository;

namespace ShoppingPlatform.DAL;

public static class DataAccessDependencyInjection
{
    public static void DataAccessInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("default"));
        });

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IWishlistRepository, WishListRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}