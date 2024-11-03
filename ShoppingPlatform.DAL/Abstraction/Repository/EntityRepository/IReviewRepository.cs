using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;

public interface IReviewRepository : IRepository<Review>
{
    Task<List<Review>> GetReviewsByProductId(int productId);
}