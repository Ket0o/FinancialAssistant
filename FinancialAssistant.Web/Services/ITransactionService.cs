using FinancialAssistant.DataTransfer.Transaction;

namespace FinancialAssistant.Web.Services;

public interface ITransactionService
{
    Task AddTransaction(AddTransactionDto transaction);
    Task DeleteTransaction(long id, CancellationToken cancellationToken);
    Task ChangeTransaction(UpdateTransactionDto updateTransaction,
        CancellationToken cancellationToken);
    Task<IEnumerable<GetTransactionDto>?> GetAllTransactions(CancellationToken cancellationToken);
    Task<GetTransactionDto?> GetTransaction(long transactionId,
        CancellationToken cancellationToken);
    Task<IEnumerable<GetTransactionDto>?> GetMonthTransactions(int month, 
        CancellationToken cancellationToken);
}