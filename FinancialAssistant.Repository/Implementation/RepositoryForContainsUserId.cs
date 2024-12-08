using System.Linq.Expressions;
using System.Reflection;
using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.UserIdentity;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistant.Repository.Implementation;

public class RepositoryForContainsUserId<T> : Repository<T>, IRepositoryForContainsUserId<T> where T : class, IContainsUserId
{
    private readonly DataContext _dbContext;
    private readonly IUserIdentityService _identityService;
 
    public RepositoryForContainsUserId(DataContext dbContext, 
        IUserIdentityService identityService) : base(dbContext)
    {
        _dbContext = dbContext;
        _identityService = identityService;
    }
    
    public new async Task<List<T>?> GetAllAsync(Expression<Func<T, bool>>? predicate = null, 
        PropertyInfo? propertyInfo = null,
        CancellationToken cancellationToken = default, params Expression<Func<T, object?>>[] include)
    {
        var userId = _identityService.GetUserId();
        
        var query = _dbContext.Set<T>()
            .AsNoTracking()
            .AsQueryable()
            .Where(data => data.UserId == userId);
        
        query = include.Aggregate(query, (current, inc) => current.Include(inc));

        if (predicate is { } notNullPredicate)
            query = query.Where(notNullPredicate);
        
        if (propertyInfo is { })
            query = query.OrderBy(entity => propertyInfo.GetValue(entity));
       
        return await query.ToListAsync(cancellationToken);
    }

    public new async Task<T?> GetAsync(Expression<Func<T, bool>>? predicate = null, 
        CancellationToken cancellationToken = default, params Expression<Func<T, object?>>[] include)
    {
        var userId = _identityService.GetUserId();
        
        var query = _dbContext.Set<T>()
            .AsNoTracking()
            .AsQueryable()
            .Where(data => data.UserId == userId);
            
        query = include.Aggregate(query, (current, inc) => current.Include(inc));

        if (predicate is { } notNullPredicate)
            query = query.Where(notNullPredicate);

        return await query.FirstOrDefaultAsync(cancellationToken); 
    }

    public new async Task AddAsync(T entity)
    {
        var userId = _identityService.GetUserId();
        
        entity.UserId = userId;
        _ = await _dbContext.Set<T>().AddAsync(entity);
        _ = await _dbContext.SaveChangesAsync();
    }

    public new async Task AddRangeAsync(IEnumerable<T> entities)
    {
        var userId = _identityService.GetUserId();
        
        foreach (var entity in entities)
        {
            entity.UserId = userId;
        }
        await _dbContext.Set<T>().AddRangeAsync(entities);
        _ = await _dbContext.SaveChangesAsync();
    }
}