using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Account;
using FinancialAssistant.Repository;
using OneOf;
using OneOf.Types;

namespace FinancialAssistant.Web.Services.Implementation;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(
        IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<OneOf<Success<List<GetAccountDto>>, Error<string>>> GetAllAccounts(
        CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAllAsync(cancellationToken: cancellationToken) is not { } accounts)
            return new Error<string>("Счетов нет.");

        return new Success<List<GetAccountDto>>(
            accounts.Select(account => new GetAccountDto(account.Id, account.Name, account.Balance, account.IsDefault))
                .ToList());
    }

    public async Task<OneOf<Success<GetAccountDto>, Error<string>>> GetAccount(long accountId, 
        CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(account => account.Id == accountId, cancellationToken) is not { } account)
            return new Error<string> ("Данного счета не существует.");

        return new Success<GetAccountDto>(
            new GetAccountDto(accountId, account.Name, account.Balance, account.IsDefault));
    }

    public async Task<OneOf<Success<string>, Error<string>>> AddAccount(string name, 
        CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(account => account.Name == name, cancellationToken) is {})
            return new Error<string>("Счет с таким именем уже существует.");
        
        if (await _accountRepository.CountAccounts(cancellationToken) == 3)
            return new Error<string>("Достигнут лимит по счетам, удалите один из счетов и попробуйте еще раз.");

        if (await _accountRepository.CountAccounts(cancellationToken) == 0)
        {
            await _accountRepository.AddAsync(new Account {Balance = 0, Name = name, IsDefault = true});
            return new Success<string>("Счет успешно создан.");
        }
        
        await _accountRepository.AddAsync(new Account {Balance = 0, Name = name});
        return new Success<string>("Счет успешно создан.");
    }

    public async Task<OneOf<Success<string>, Error<string>>> EditAccount(UpdateAccountDto updateAccount, 
        CancellationToken cancellationToken)
    {
        if (await _accountRepository
                .GetAsync(account => account.Id == updateAccount.Id, cancellationToken) is not {} account)
            return new Error<string> ("Данного счета не существует.");
        
        if (!updateAccount.IsDefault)
        {
            account.Name = updateAccount.Name;
            await _accountRepository.UpdateAsync(account);
            return new Success<string>("Счет успешно изменен.");
        }
        
        if (account.IsDefault)
        {
            account.Name = updateAccount.Name;
            await _accountRepository.UpdateAsync(account);
            return new Success<string>("Счет успешно изменен.");
        }
        
        if (await _accountRepository
                .GetAsync(account => account.IsDefault, cancellationToken) is { } defaultAccount)
        {
            defaultAccount.IsDefault = false;
            await _accountRepository.UpdateAsync(defaultAccount);
        }

        account.IsDefault = updateAccount.IsDefault;
        account.Name = updateAccount.Name;
        await _accountRepository.UpdateAsync(account);
        return new Success<string>("Счет успешно изменен.");
    }

    public async Task<OneOf<Success<string>, Error<string>>> UpdateAccountBalance(
        UpdateAccountBalanceDto updateAccountBalance, CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(account => account.Id == updateAccountBalance.Id, cancellationToken) is
            not { } account)
            return new Error<string>("Данного счета не существует.");

        account.Balance += updateAccountBalance.Amount;
        await _accountRepository.UpdateAsync(account);
        return new Success<string>("Баланс счета успешно обновлен.");
    }

    public async Task<OneOf<Success<string>, Error<string>>> AddFirstAccount(long userId, string name, CancellationToken cancellationToken)
    {
        await _accountRepository.FirstAddAsync(new Account {UserId = userId, Balance = 0, Name = name, IsDefault = true});
        return new Success<string>("Счет успешно создан.");
    }

    public async Task<OneOf<Success<string>, Error<string>>> DeleteAccount(long id, CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(account => account.Id == id, cancellationToken) is not {} account)
            return new Error<string> ("Данного счета не существует.");

        await _accountRepository.DeleteAsync(account);
        return new Success<string>("Счет успешно удален.");
    }

    public async Task<OneOf<Success<GetAccountDto>, Error<string>>> GetDefaultAccount(
        CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetAsync(account => account.IsDefault, cancellationToken) is not {} account)
            return new Error<string> ("Нет основного счета.");

        return new Success<GetAccountDto>(new GetAccountDto(account.Id, account.Name, account.Balance,
            account.IsDefault));
    }
}