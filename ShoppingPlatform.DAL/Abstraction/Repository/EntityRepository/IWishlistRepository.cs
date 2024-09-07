using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;

public interface IWishlistRepository : IRepository<WishList>
{
    Task<WishList> GetUserWishlist(int userId);
}