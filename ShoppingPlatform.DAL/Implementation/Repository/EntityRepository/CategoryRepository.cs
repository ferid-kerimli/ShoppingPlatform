using ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;
using ShoppingPlatform.DAL.Context;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Implementation.Repository.EntityRepository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}