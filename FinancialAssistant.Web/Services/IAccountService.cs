using FinancialAssistant.DataTransfer.Account;
using OneOf;
using OneOf.Types;

namespace FinancialAssistant.Web.Services;

public interface IAccountService
{
    Task<OneOf<Success<List<GetAccountDto>>, Error<string>>> GetAllAccounts(CancellationToken cancellationToken);

    Task<OneOf<Success<GetAccountDto>, Error<string>>> GetAccount(long accountId, CancellationToken cancellationToken);
    
    Task<OneOf<Success<string>, Error<string>>> AddAccount(string name, CancellationToken cancellationToken);
    
    Task<OneOf<Success<string>, Error<string>>> EditAccount(UpdateAccountDto updateAccount,
        CancellationToken cancellationToken);

    Task<OneOf<Success<string>, Error<string>>> DeleteAccount(long id, CancellationToken cancellationToken);

    Task<OneOf<Success<GetAccountDto>, Error<string>>> GetDefaultAccount(
        CancellationToken cancellationToken);

    Task<OneOf<Success<string>, Error<string>>> UpdateAccountBalance(
        UpdateAccountBalanceDto updateAccountBalance, CancellationToken cancellationToken);
    
    Task<OneOf<Success<string>, Error<string>>> AddFirstAccount(long userId, string name, CancellationToken cancellationToken);
}