using System.Linq.Expressions;

namespace FinancialAssistant.Repository;

public interface IRepository<T> where T : class
{
    Task<List<T>?> GetAllAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken);

    Task<T?> GetAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken);

    Task AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);
    
    Task UpdateAsync(T entity);
    
    Task UpdateRangeAsync(IEnumerable<T> entities);

    Task DeleteAsync(T entity);
    
    Task DeleteRangeAsync(IEnumerable<T> entities);
}