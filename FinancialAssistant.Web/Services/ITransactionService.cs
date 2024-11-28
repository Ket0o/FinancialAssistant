using FinancialAssistant.DataTransfer.Transaction;

namespace FinancialAssistant.Web.Services;

public interface ITransactionService
{
    Task AddTransaction(AddTransactionDto transaction);
    
    Task DeleteTransaction(long id, CancellationToken cancellationToken);
    
    Task ChangeTransaction(UpdateTransactionDto updateTransaction,
        CancellationToken cancellationToken);
    
    Task<List<GetTransactionDto>?> GetAllTransactions(CancellationToken cancellationToken);
    
    Task<GetTransactionDto?> GetTransaction(long transactionId,
        CancellationToken cancellationToken);
    
    Task<List<GetTransactionDto>?> GetMonthAndAccountTransactions(int month, 
        CancellationToken cancellationToken);

    Task<List<GetTransactionDto>?> GetLastTenTransactions(CancellationToken cancellationToken);
}