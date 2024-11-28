using FinancialAssistant.DataAccess.Model;

namespace FinancialAssistant.Repository;

public interface ITransactionRepository : IRepositoryForContainsUserId<Transaction>
{
     Task<List<Transaction>?> GetLastTen(CancellationToken cancellationToken);
}