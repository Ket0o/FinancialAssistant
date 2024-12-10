using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Account;
using FinancialAssistant.DataTransfer.Transaction;
using FinancialAssistant.Repository;
using FinancialAssistant.Web.Extensions;
using FinancialAssistant.Web.Mapping.Transactions;
using OneOf;
using OneOf.Types;
using NotFound = OneOf.Types.NotFound;

namespace FinancialAssistant.Web.Services.Implementation;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountService _accountService;
    
    public TransactionService(ITransactionRepository transactionRepository, 
        IAccountService accountService)
    {
        _transactionRepository = transactionRepository;
        _accountService = accountService;
    }

    public async Task<OneOf<Success, NotFound>> DeleteTransaction(long id, 
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository.GetAsync(transactionFromDataBase => transactionFromDataBase.Id == id, 
            cancellationToken) is not { } transaction)
            return new NotFound();
        
        await _transactionRepository.DeleteAsync(transaction);
        await _accountService.UpdateAccountBalance(new UpdateAccountBalanceDto(transaction.AccountId,
            -transaction.Amount), cancellationToken);
        
        return new Success();
    }

    public async Task<OneOf<Success, NotFound>> ChangeTransaction(UpdateTransactionDto updateTransaction, 
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository
            .GetAsync(transactionFromDataBase => 
                transactionFromDataBase.Id == updateTransaction.Id, cancellationToken) is not { } transaction)
            return new NotFound();

        await _accountService.UpdateAccountBalance(new UpdateAccountBalanceDto(transaction.AccountId,
            -transaction.Amount), cancellationToken);
        
        await _transactionRepository.UpdateAsync(updateTransaction.ToModel());
        
        await _accountService.UpdateAccountBalance(new UpdateAccountBalanceDto(updateTransaction.Id,
            updateTransaction.Amount), cancellationToken);
        return new Success();
    }

    public async Task<OneOf<Success<List<GetTransactionDto>>, NotFound>> GetAllTransactions(TransactionFilterDto filter,
        CancellationToken cancellationToken)
    {
        Expression<Func<Transaction, bool>> predicate = t => true;

        if (filter.AccountId.HasValue)
            predicate = predicate.AndAlso(transaction => transaction.AccountId == filter.AccountId);

        if (filter.CategoryId.HasValue) 
            predicate = predicate.AndAlso(transaction => transaction.CategoryId == filter.CategoryId);
        
        if (!string.IsNullOrEmpty(filter.YearMonth))
        {
            if (DateTime.TryParseExact(filter.YearMonth, "yyyy-MM", CultureInfo.InvariantCulture, 
                    DateTimeStyles.None,
                    out var date))
            {
                predicate = predicate.AndAlso(transaction =>
                    transaction.TransactionDate.Year == date.Year && transaction.TransactionDate.Month == date.Month);
            }
        }

        PropertyInfo? propertyInfo = null;
        if (!string.IsNullOrEmpty(filter.SortBy)) propertyInfo = typeof(Transaction).GetProperty(filter.SortBy);

        if (await _transactionRepository.GetAllAsync(predicate, propertyInfo, cancellationToken) is 
            not { } transactions)
            return new NotFound();

        return new Success<List<GetTransactionDto>>(transactions
            .Select(transaction => transaction.ToGetTransactionDto()).ToList());
    }

    public async Task<OneOf<Success<GetTransactionDto>, NotFound>> GetTransaction(long transactionId, 
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository.GetAsync(
                transactionFromDataBase => transactionFromDataBase.Id == transactionId,
                cancellationToken) is not { } transaction)
            return new NotFound();

        return new Success<GetTransactionDto>(transaction.ToGetTransactionDto());
    }

    public async Task<OneOf<Success<List<GetTransactionDto>>, NotFound>> GetLastTenTransactions(
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository.GetLastTen(cancellationToken) is not { } transactions)
            return new NotFound();

        return new Success<List<GetTransactionDto>>(transactions.Select(transaction => 
            transaction.ToGetTransactionDto()).ToList());
    }

    public async Task AddTransaction(AddTransactionDto transaction, CancellationToken cancellationToken)
    {
        await _transactionRepository.AddAsync(transaction.ToModel());
        
        await _accountService.UpdateAccountBalance(new UpdateAccountBalanceDto(transaction.AccountId,
            -transaction.Amount), cancellationToken);
    }
}
