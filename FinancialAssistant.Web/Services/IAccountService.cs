using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Account;

namespace FinancialAssistant.Web.Services;

public interface IAccountService
{
    Task<List<GetAccountDto>?> GetAllAccounts(CancellationToken cancellationToken);

    Task<GetAccountDto?> GetAccount(long accountId, CancellationToken cancellationToken);
    
    Task AddAccount(string name, CancellationToken cancellationToken);
    
    Task EditAccount(UpdateAccountDto updateAccount, CancellationToken cancellationToken);

    Task DeleteAccount(long id, CancellationToken cancellationToken);
}