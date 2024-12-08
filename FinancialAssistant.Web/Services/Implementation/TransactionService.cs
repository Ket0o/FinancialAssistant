using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Account;
using FinancialAssistant.DataTransfer.Transaction;
using FinancialAssistant.Repository;
using FinancialAssistant.UserIdentity;
using FinancialAssistant.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

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

    public async Task DeleteTransaction(long id, CancellationToken cancellationToken)
    {
        if (await _transactionRepository.GetAsync(transaction => transaction.Id == id, 
            cancellationToken) is not { } transaction)
            return;
        
        await _transactionRepository.DeleteAsync(transaction);
        await _accountService.UpdateAccountBalance(new UpdateAccountBalanceDto(transaction.AccountId,
            -transaction.Amount), cancellationToken);
    }

    public async Task ChangeTransaction(UpdateTransactionDto updateTransaction, 
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository
            .GetAsync(transaction => 
            transaction.Id == updateTransaction.Id, cancellationToken) is not { } transaction)
            return;

        await _accountService.UpdateAccountBalance(new UpdateAccountBalanceDto(transaction.AccountId,
            -transaction.Amount), cancellationToken);
        
        await _transactionRepository.UpdateAsync(new DataAccess.Model.Transaction
        {
            AccountId = updateTransaction.Id,
            CategoryId = updateTransaction.CategoryId,
            Amount = updateTransaction.Amount,
            TransactionDate = updateTransaction.TransactionDate,
            Description = updateTransaction.Description
        });
        
        await _accountService.UpdateAccountBalance(new UpdateAccountBalanceDto(updateTransaction.Id,
            updateTransaction.Amount), cancellationToken);
    }

    public async Task<List<GetTransactionDto>?> GetAllTransactions(TransactionFilterDto filter,
        CancellationToken cancellationToken)
    {
        Expression<Func<Transaction, bool>> predicate = t => true;

        if (filter.AccountId.HasValue)
        {
            predicate = predicate.AndAlso(transaction => transaction.AccountId == filter.AccountId);
        }

        if (filter.CategoryId.HasValue)
        {
            predicate = predicate.AndAlso(transaction => transaction.CategoryId == filter.CategoryId);
        }
        
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
        
        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            propertyInfo = typeof(Transaction).GetProperty(filter.SortBy);
        }

        if (await _transactionRepository.GetAllAsync(predicate, propertyInfo, cancellationToken) is 
            not { } transactions)
            return default;
        
        return transactions.Select(transaction => new GetTransactionDto(transaction.Id, transaction.Name,
            transaction.AccountId, transaction.CategoryId, transaction.Amount,
            transaction.TransactionDate, transaction.Description, transaction.CreatedAt, transaction.Account.Name, 
            transaction.Category.Name, transaction.Category.Color)).ToList();
    }

    public async Task<GetTransactionDto?> GetTransaction(long transactionId, 
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository.GetAsync(transaction => transaction.Id == transactionId, 
            cancellationToken) is not { } transaction)
            return null;

        return new GetTransactionDto(transaction.Id, transaction.Name, transaction.AccountId, transaction.CategoryId, 
            transaction.Amount, transaction.TransactionDate, transaction.Description, 
            transaction.CreatedAt, transaction.Account.Name, transaction.Category.Name, transaction.Category.Color);
    }

    public async Task<List<GetTransactionDto>?> GetLastTenTransactions(CancellationToken cancellationToken)
    {
        if (await _transactionRepository.GetLastTen(cancellationToken) is not { } transactions)
            return null;

        return transactions.Select(transaction => new GetTransactionDto(transaction.Id, transaction.Name,
                transaction.AccountId, transaction.CategoryId, transaction.Amount, transaction.TransactionDate,
                transaction.Description, transaction.CreatedAt, transaction.Account.Name, transaction.Category.Name,
                transaction.Category.Color))
            .ToList();
    }

    public async Task AddTransaction(AddTransactionDto transaction, CancellationToken cancellationToken)
    {
        await _transactionRepository.AddAsync(new DataAccess.Model.Transaction
        {
            AccountId = transaction.AccountId,
            CategoryId = transaction.CategoryId,
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            Description = transaction.Description,
            CreatedAt = DateTime.Now
        });
        
        await _accountService.UpdateAccountBalance(new UpdateAccountBalanceDto(transaction.AccountId,
            -transaction.Amount), cancellationToken);
    }
}
