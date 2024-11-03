using Microsoft.EntityFrameworkCore;
using ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;
using ShoppingPlatform.DAL.Context;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Implementation.Repository.EntityRepository;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    private readonly ApplicationDbContext _context;
    
    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<List<Review>> GetReviewsByProductId(int productId)
    {
        return await _context.Reviews
            .Where(r => r.ProductId == productId)
            .ToListAsync();
    }
}