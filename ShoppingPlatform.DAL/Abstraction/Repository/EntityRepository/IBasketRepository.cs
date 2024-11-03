using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;

public interface IBasketRepository : IRepository<Basket>
{
    Task<Basket> GetUserBasket(int userId);
}