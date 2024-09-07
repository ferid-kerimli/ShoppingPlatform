namespace ShoppingPlatform.DAL.Abstraction.Repository;

public interface IRepository<T>
{
    IQueryable<T> GetAll();
    Task<T> GetById(int id);
    Task<bool> Create(T data);
    bool Delete(T data);
    Task<bool> DeleteById(int id);
    bool Update(T data);
}