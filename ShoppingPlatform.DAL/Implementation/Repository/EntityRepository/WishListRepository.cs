using Microsoft.EntityFrameworkCore;
using ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;
using ShoppingPlatform.DAL.Context;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Implementation.Repository.EntityRepository;

public class WishListRepository : Repository<WishList>, IWishlistRepository
{
    private readonly ApplicationDbContext _context;
    
    public WishListRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<WishList> GetUserWishlist(int userId)
    {
        return await _context.WishLists
            .Include(w => w.WishlistItems)
            .ThenInclude(wi => wi.Product)
            .FirstOrDefaultAsync(w => w.UserId == userId);
    }
}