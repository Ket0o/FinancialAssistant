using FinancialAssistant.DataTransfer.Transaction;
using FinancialAssistant.Repository;

namespace FinancialAssistant.Web.Services.Implementation;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task DeleteTransaction(long id, CancellationToken cancellationToken)
    {
        if (await _transactionRepository.GetAsync(transaction => transaction.Id == id, 
            cancellationToken) is not { } transaction)
            return;

        await _transactionRepository.DeleteAsync(transaction);
    }

    public async Task ChangeTransaction(UpdateTransactionDto updateTransaction, 
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository
            .GetAsync(transaction => 
            transaction.Id == updateTransaction.Id, cancellationToken) is not { })
            return;

        await _transactionRepository.UpdateAsync(new DataAccess.Model.Transaction
        {
            AccountId = updateTransaction.Id,
            CategoryId = updateTransaction.CategoryId,
            Amount = updateTransaction.Amount,
            TransactionDate = updateTransaction.TransactionDate,
            Description = updateTransaction.Description
        });
    }

    public async Task<IEnumerable<GetTransactionDto>?> GetAllTransactions(
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository.GetAllAsync(cancellationToken: cancellationToken) is 
            not { } transactions)
            return default;

        return transactions.Select(transaction => new GetTransactionDto(transaction.Id,
            transaction.AccountId, transaction.CategoryId, transaction.Amount,
            transaction.TransactionDate, transaction.Description, transaction.CreatedAt));
    }

    public async Task<GetTransactionDto?> GetTransaction(long transactionId, 
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository.GetAsync(transaction => transaction.Id == transactionId, 
            cancellationToken) is not { } transaction)
            return null;

        return new GetTransactionDto(transaction.Id, transaction.AccountId, transaction.CategoryId, 
            transaction.Amount, transaction.TransactionDate, transaction.Description, 
            transaction.CreatedAt);
    }

    public async Task<IEnumerable<GetTransactionDto>?> GetMonthTransactions(int month, 
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository
            .GetAllAsync(transaction => transaction.TransactionDate.Month == month, 
            cancellationToken) is not { } transactions)
            return default;

        return transactions.Select(transaction => new GetTransactionDto(transaction.Id,
            transaction.AccountId, transaction.CategoryId, transaction.Amount,
            transaction.TransactionDate, transaction.Description, transaction.CreatedAt));
    }

    public async Task AddTransaction(AddTransactionDto transaction)
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
    }
}
