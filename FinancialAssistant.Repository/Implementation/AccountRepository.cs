using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.UserIdentity;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistant.Repository.Implementation;

public class AccountRepository : RepositoryForContainsUserId<Account>, IAccountRepository
{
    private readonly DataContext _dataContext;
    private readonly long _userId;
    
    public AccountRepository(DataContext dataContext, IUserIdentityService identityService) 
        : base(dataContext, identityService)
    {
        _dataContext = dataContext;
        _userId = identityService.GetUserId();
    }

    public async Task<int> CountAccounts(CancellationToken cancellationToken)
        => await _dataContext.Accounts.AsNoTracking().AsQueryable().Where(account => account.UserId == _userId)
            .CountAsync(cancellationToken: cancellationToken);
}