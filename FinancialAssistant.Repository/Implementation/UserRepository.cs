using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistant.Repository.Implementation;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly DataContext _dbContext;
    
    public UserRepository(DataContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ItExistingUser(string email, CancellationToken cancellationToken)
        => await _dbContext.Users
            .AsNoTracking()
            .AsQueryable()
            .CountAsync(user => EF.Functions.ILike(user.Email, email), cancellationToken) > 0;
}