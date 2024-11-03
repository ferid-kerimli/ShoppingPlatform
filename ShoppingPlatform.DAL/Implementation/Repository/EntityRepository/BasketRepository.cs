using Microsoft.EntityFrameworkCore;
using ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;
using ShoppingPlatform.DAL.Context;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Implementation.Repository.EntityRepository;

public class BasketRepository : Repository<Basket>, IBasketRepository
{
    private readonly ApplicationDbContext _context;
    
    public BasketRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Basket> GetUserBasket(int userId)
    {
        return await _context.Baskets
            .Include(b => b.BasketItems)
            .ThenInclude(bi => bi.Product)
            .FirstOrDefaultAsync(b => b.UserId == userId);
    }
}