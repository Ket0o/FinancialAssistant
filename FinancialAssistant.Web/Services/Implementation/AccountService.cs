using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Account;
using FinancialAssistant.Repository;

namespace FinancialAssistant.Web.Services.Implementation;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(
        IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<List<GetAccountDto>?> GetAllAccounts(CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAllAsync(null, cancellationToken) is not { } accounts)
            return default;

        return accounts.Select(account => new GetAccountDto(account.Id, account.Name, account.Balance)).ToList();
    }

    public async Task<GetAccountDto?> GetAccount(long accountId, CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(account => account.Id == accountId, cancellationToken) is not { } account)
            return default;

        return new GetAccountDto(accountId, account.Name, account.Balance);
    }

    public async Task AddAccount(string name, CancellationToken cancellationToken)
    {
        if (await _accountRepository.CountAccounts(cancellationToken) == 3)
            return;

        await _accountRepository.AddAsync(new Account {Balance = 0, Name = name});
    }

    public async Task EditAccount(UpdateAccountDto updateAccount, CancellationToken cancellationToken)
    {
        if (await _accountRepository
                .GetAsync(account => account.Id == updateAccount.Id, cancellationToken) is not {} account)
            return;

        account.Name = updateAccount.Name;
        await _accountRepository.UpdateAsync(account);
    }

    public async Task DeleteAccount(long id, CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(account => account.Id == id, cancellationToken) is not {} account)
            return;

        await _accountRepository.DeleteAsync(account);
    }
}