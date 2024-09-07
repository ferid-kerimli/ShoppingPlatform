using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;

public interface IRatingRepository : IRepository<Rating>
{
    Task<decimal> GetAverageRating(int productId);
    Task<IEnumerable<Rating>> GetRatingsByProductId(int productId);
}