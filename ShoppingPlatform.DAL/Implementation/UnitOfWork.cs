using ShoppingPlatform.DAL.Abstraction;
using ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;
using ShoppingPlatform.DAL.Context;

namespace ShoppingPlatform.DAL.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IBasketRepository BasketRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IWishlistRepository WishlistRepository { get; }
    public IRatingRepository RatingRepository { get; }
    public IReviewRepository ReviewRepository { get; }

    public UnitOfWork(IBasketRepository basketRepository, ICategoryRepository categoryRepository, IProductRepository productRepository, IWishlistRepository wishlistRepository, ApplicationDbContext context, IRatingRepository ratingRepository, IReviewRepository reviewRepository)
    {
        BasketRepository = basketRepository;
        CategoryRepository = categoryRepository;
        ProductRepository = productRepository;
        WishlistRepository = wishlistRepository;
        _context = context;
        RatingRepository = ratingRepository;
        ReviewRepository = reviewRepository;
    }
    
    public async Task<int> Commit()
    {
        return await _context.SaveChangesAsync();
    }
}