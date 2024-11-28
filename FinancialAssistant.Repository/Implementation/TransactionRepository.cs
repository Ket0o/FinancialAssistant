using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.UserIdentity;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistant.Repository.Implementation;

public class TransactionRepository : RepositoryForContainsUserId<Transaction>, ITransactionRepository
{
    private readonly DataContext _dataContext;
    
    public TransactionRepository(DataContext dataContext, IUserIdentityService identityService) 
        : base(dataContext, identityService)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Transaction>?> GetLastTen(CancellationToken cancellationToken)
        => await _dataContext.Transactions.AsNoTracking().AsQueryable()
            .OrderByDescending(transaction => transaction.TransactionDate).Take(10).ToListAsync(cancellationToken);
}