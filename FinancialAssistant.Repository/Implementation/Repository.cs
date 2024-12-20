﻿using System.Linq.Expressions;
using System.Reflection;
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

    public async Task<List<T>?> GetAllAsync(Expression<Func<T, bool>>? predicate = null, 
        PropertyInfo? propertyInfo = null,
        CancellationToken cancellationToken = default, params Expression<Func<T, object?>>[] include)
    {
        var query = _dbContext.Set<T>()
            .AsNoTracking()
            .AsQueryable();
        
        query = include.Aggregate(query, (current, inc) => current.Include(inc));
        
        if (predicate is { } notNullPredicate)
            query = query.Where(notNullPredicate);

        if (propertyInfo is { })
            query = query.OrderBy(entity => propertyInfo.GetValue(entity));
       
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>>? predicate = null, 
        CancellationToken cancellationToken = default, params Expression<Func<T, object?>>[] include)
    {
        var query = _dbContext.Set<T>()
            .AsNoTracking()
            .AsQueryable();
        
        query = include.Aggregate(query, (current, inc) => current.Include(inc));

        if (predicate is { } notNullPredicate)
            query = query.Where(notNullPredicate);

        return await query.FirstOrDefaultAsync(cancellationToken); 
    }

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