using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Account;
using FinancialAssistant.Repository;
using FinancialAssistant.Web.Mapping.Accounts;
using OneOf;
using OneOf.Types;
using NotFound = OneOf.Types.NotFound;

namespace FinancialAssistant.Web.Services.Implementation;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(
        IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<OneOf<Success<List<GetAccountDto>>, NotFound>> GetAllAccounts(
        CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAllAsync(cancellationToken: cancellationToken) is not { } accounts)
            return new NotFound();

        return new Success<List<GetAccountDto>>(
            accounts.Select(account => new GetAccountDto(account.Id, account.Name, account.Balance, account.IsDefault))
                .ToList());
    }

    public async Task<OneOf<Success<GetAccountDto>, NotFound>> GetAccount(long accountId, 
        CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(accountFromDataBase => accountFromDataBase.Id == accountId,
                cancellationToken) is not { } account)
            return new NotFound();

        return new Success<GetAccountDto>(
            new GetAccountDto(accountId, account.Name, account.Balance, account.IsDefault));
    }

    public async Task<OneOf<Success<long>, Error<string>>> AddAccount(string name, 
        CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(account => account.Name == name, cancellationToken) is {})
            return new Error<string>("Счет с таким именем уже существует.");
        
        if (await _accountRepository.CountAccounts(cancellationToken) == 3)
            return new Error<string>("Достигнут лимит по счетам, удалите один из счетов и попробуйте еще раз.");

        var account = new Account {Balance = 0, Name = name};
        if (await _accountRepository.CountAccounts(cancellationToken) == 0)
        {
            account.IsDefault = true;
            await _accountRepository.AddAsync(account);
            return new Success<long>(account.Id);
        }
        
        await _accountRepository.AddAsync(account);
        return new Success<long>(account.Id);
    }

    public async Task<OneOf<Success, Error<string>, NotFound>> EditAccount(UpdateAccountDto updateAccount, 
        CancellationToken cancellationToken)
    {
        if (await _accountRepository
                .GetAsync(accountFromDataBase => accountFromDataBase.Id == updateAccount.Id, cancellationToken) is not
            { } account)
            return new NotFound();
        
        if (!updateAccount.IsDefault)
        {
            if (account.IsDefault)
                return new Error<string> ("Установите в качестве счета по умолчанию другой счет.");
            account.Name = updateAccount.Name;
            await _accountRepository.UpdateAsync(account);
            return new Success();
        }
        
        if (account.IsDefault)
        {
            account.Name = updateAccount.Name;
            await _accountRepository.UpdateAsync(account);
            return new Success();
        }
        
        if (await _accountRepository
                .GetAsync(accountFromDataBase => accountFromDataBase.IsDefault, cancellationToken) is { } defaultAccount)
        {
            defaultAccount.IsDefault = false;
            await _accountRepository.UpdateAsync(defaultAccount);
        }

        account.IsDefault = updateAccount.IsDefault;
        account.Name = updateAccount.Name;
        await _accountRepository.UpdateAsync(account);
        return new Success();
    }

    public async Task<OneOf<Success, NotFound>> UpdateAccountBalance(
        UpdateAccountBalanceDto updateAccountBalance, CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(accountFromDataBase => accountFromDataBase.Id == updateAccountBalance.Id,
                cancellationToken) is not { } account)
            return new NotFound();

        account.Balance += updateAccountBalance.Amount;
        await _accountRepository.UpdateAsync(account);
        return new Success();
    }

    public async Task AddFirstAccount(AddFirstAccountDto addFirstAccount, CancellationToken cancellationToken)
        => await _accountRepository.FirstAddAsync(addFirstAccount.ToModel());

    public async Task<OneOf<Success, NotFound>> DeleteAccount(long id, CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(accountFromDataBase => accountFromDataBase.Id == id, cancellationToken) is
            not { } account)
            return new NotFound();

        await _accountRepository.DeleteAsync(account);
        return new Success();
    }

    public async Task<OneOf<Success<GetAccountDto>, NotFound>> GetDefaultAccount(
        CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(accountFromDataBase => accountFromDataBase.IsDefault, cancellationToken)
            is not { } account)
            return new NotFound();

        return new Success<GetAccountDto>(new GetAccountDto(account.Id, account.Name, account.Balance,
            account.IsDefault));
    }
}