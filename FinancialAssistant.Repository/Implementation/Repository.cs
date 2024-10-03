using System.Linq.Expressions;
using FinancialAssistant.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistant.Repository.Implementation;

public class Repository<T> : IRepository<T> where T : class
{ 
    private readonly DataContext _dbContext;

    public Repository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<T>?> GetAllAsync(Expression<Func<T, bool>> predicate, 
        CancellationToken cancellationToken) 
        => await _dbContext.Set<T>()
            .AsNoTracking()
            .AsQueryable()
            .Where(predicate)
            .ToListAsync(cancellationToken: cancellationToken);

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        => await _dbContext.Set<T>()
            .AsNoTracking()
            .AsQueryable()
            .FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task AddAsync(T entity)
    {
        _ = await _dbContext.Set<T>().AddAsync(entity);
        _ = await _dbContext.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        _ = await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _ = _dbContext.Set<T>().Update(entity);
        _ = await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().UpdateRange(entities);
        _ = await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _ = _dbContext.Set<T>().Remove(entity);
        _ = await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        _ = await _dbContext.SaveChangesAsync();
    }
}