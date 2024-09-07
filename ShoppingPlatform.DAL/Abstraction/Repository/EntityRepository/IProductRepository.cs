using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.DAL.Abstraction.Repository.EntityRepository;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllProducts();
    Task<List<Product>> GetAllProductsByCategoryId(int categoryId);
    Task<Product> GetProductById(int id);
    Task<List<Product>> GetProductsByUserId(int userId);
    Task<Product> GetProductByUserId(int userId, int productId);
    Task<List<Product>> GetProductsByUserCategoryId(int userId, int categoryId);
}