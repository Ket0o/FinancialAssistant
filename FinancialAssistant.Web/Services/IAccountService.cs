using FinancialAssistant.DataTransfer.Account;
using OneOf;
using OneOf.Types;

namespace FinancialAssistant.Web.Services;

public interface IAccountService
{
    Task<OneOf<Success<List<GetAccountDto>>, NotFound>> GetAllAccounts(CancellationToken cancellationToken);

    Task<OneOf<Success<GetAccountDto>, NotFound>> GetAccount(long accountId, CancellationToken cancellationToken);
    
    Task<OneOf<Success<long>, Error<string>>> AddAccount(string name, CancellationToken cancellationToken);
    
    Task<OneOf<Success, Error<string>, NotFound>> EditAccount(UpdateAccountDto updateAccount,
        CancellationToken cancellationToken);

    Task<OneOf<Success, NotFound>> DeleteAccount(long id, CancellationToken cancellationToken);

    Task<OneOf<Success<GetAccountDto>, NotFound>> GetDefaultAccount(
        CancellationToken cancellationToken);

    Task<OneOf<Success, NotFound>> UpdateAccountBalance(
        UpdateAccountBalanceDto updateAccountBalance, CancellationToken cancellationToken);

    Task AddFirstAccount(AddFirstAccountDto addFirstAccount, CancellationToken cancellationToken);
}