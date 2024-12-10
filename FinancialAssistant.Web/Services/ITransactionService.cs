using FinancialAssistant.DataTransfer.Account;
using FinancialAssistant.DataTransfer.Transaction;
using OneOf;
using OneOf.Types;

namespace FinancialAssistant.Web.Services;

public interface ITransactionService
{
    Task AddTransaction(AddTransactionDto transaction, CancellationToken cancellationToken);
    
    Task<OneOf<Success, NotFound>> DeleteTransaction(long id,
        CancellationToken cancellationToken);
    
    Task<OneOf<Success, NotFound>> ChangeTransaction(UpdateTransactionDto updateTransaction,
        CancellationToken cancellationToken);

    Task<OneOf<Success<List<GetTransactionDto>>, NotFound>> GetAllTransactions(TransactionFilterDto filter,
        CancellationToken cancellationToken);
    
    Task<OneOf<Success<GetTransactionDto>, NotFound>> GetTransaction(long transactionId,
        CancellationToken cancellationToken);

    Task<OneOf<Success<List<GetTransactionDto>>, NotFound>> GetLastTenTransactions(CancellationToken cancellationToken);
}