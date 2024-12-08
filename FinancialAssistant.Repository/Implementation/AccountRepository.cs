using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.UserIdentity;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistant.Repository.Implementation;

public class AccountRepository : RepositoryForContainsUserId<Account>, IAccountRepository
{
    private readonly DataContext _dataContext;
    private readonly IUserIdentityService _identityService;
    
    public AccountRepository(DataContext dataContext, IUserIdentityService identityService) 
        : base(dataContext, identityService)
    {
        _dataContext = dataContext;
        _identityService = identityService;
    }

    public async Task<int> CountAccounts(CancellationToken cancellationToken)
        => await _dataContext.Accounts.AsNoTracking().AsQueryable().Where(account => account.UserId == _identityService.GetUserId())
            .CountAsync(cancellationToken: cancellationToken);
    
    public async Task FirstAddAsync(Account entity)
    {
        _ = await _dataContext.Accounts.AddAsync(entity);
        _ = await _dataContext.SaveChangesAsync();
    }
}