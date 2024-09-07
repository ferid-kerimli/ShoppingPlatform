using Microsoft.EntityFrameworkCore;
using ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;
using ShoppingPlatform.DAL.Context;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Implementation.Repository.EntityRepository;

public class RatingRepository : Repository<Rating>, IRatingRepository
{
    private readonly ApplicationDbContext _context;
    
    public RatingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<decimal> GetAverageRating(int productId)
    {
        return (decimal) await _context.Ratings
            .Where(r => r.ProductId == productId)
            .AverageAsync(r => r.Value);
    }

    public async Task<IEnumerable<Rating>> GetRatingsByProductId(int productId)
    {
        return await _context.Set<Rating>()
            .Where(r => r.ProductId == productId)
            .ToListAsync();
    }
}