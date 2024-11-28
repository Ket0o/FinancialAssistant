using FinancialAssistant.DataTransfer.Transaction;
using FinancialAssistant.Repository;
using FinancialAssistant.UserIdentity;
using Org.BouncyCastle.Asn1.X509;

namespace FinancialAssistant.Web.Services.Implementation;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserIdentityService _identityService;
    
    public TransactionService(ITransactionRepository transactionRepository, 
        IUserIdentityService identityService)
    {
        _transactionRepository = transactionRepository;
        _identityService = identityService;
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

    public async Task<List<GetTransactionDto>?> GetAllTransactions(
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository.GetAllAsync(null, cancellationToken) is 
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

    public async Task<List<GetTransactionDto>?> GetMonthAndAccountTransactions(int month, 
        CancellationToken cancellationToken)
    {
        if (await _transactionRepository
                .GetAllAsync(transaction => transaction.TransactionDate.Month == month, 
                    cancellationToken) is not { } transactions)
            return default;

        return transactions.Select(transaction => new GetTransactionDto(transaction.Id, transaction.Name,
            transaction.AccountId, transaction.CategoryId, transaction.Amount,
            transaction.TransactionDate, transaction.Description, transaction.CreatedAt, transaction.Account.Name,
            transaction.Category.Name, transaction.Category.Color)).ToList();
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
