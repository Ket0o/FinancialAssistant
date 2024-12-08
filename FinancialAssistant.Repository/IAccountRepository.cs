using FinancialAssistant.DataAccess.Model;

namespace FinancialAssistant.Repository;

public interface IAccountRepository : IRepositoryForContainsUserId<Account>
{
    Task<int> CountAccounts(CancellationToken cancellationToken);

    Task FirstAddAsync(Account entity);
}