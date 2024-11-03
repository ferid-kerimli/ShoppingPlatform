using Microsoft.EntityFrameworkCore;
using ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;
using ShoppingPlatform.DAL.Context;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Implementation.Repository.EntityRepository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await _context.Products
            .Include(p => p.ProductImages)
            .ToListAsync();
    }

    public async Task<List<Product>> GetAllProductsByCategoryId(int categoryId)
    {
        return await _context.Products
            .Include(p => p.ProductImages)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Product> GetProductById(int id)
    {
        return await _context.Products
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException();
    }

    public async Task<List<Product>> GetProductsByUserId(int userId)
    {
        return await _context.Products
            .Include(p => p.ProductImages)
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<Product> GetProductByUserId(int userId, int productId)
    {
        return await _context.Products
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.UserId == userId && p.Id == productId) ?? throw new InvalidOperationException();
    }

    public async Task<List<Product>> GetProductsByUserCategoryId(int userId, int categoryId)
    {
        return await _context.Products
            .Include(p => p.ProductImages)
            .Where(p => p.UserId == userId && p.CategoryId == categoryId)
            .ToListAsync();
    }
}