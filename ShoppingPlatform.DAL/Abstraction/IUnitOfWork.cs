using ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;

namespace ShoppingPlatform.DAL.Abstraction;

public interface IUnitOfWork
{
    IBasketRepository BasketRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    IWishlistRepository WishlistRepository { get; }
    IRatingRepository RatingRepository { get; }
    IReviewRepository ReviewRepository { get; }
    Task<int> Commit();
}