namespace Sample.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<string> AddAsync(T entity);
    Task<string> UpdateAsync(T entity);
    Task<string> DeleteAsync(Guid id);
}
