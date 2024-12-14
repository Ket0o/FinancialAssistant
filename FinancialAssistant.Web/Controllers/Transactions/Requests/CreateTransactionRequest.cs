namespace FinancialAssistant.Web.Controllers.Transactions.Requests;

public record CreateTransactionRequest(long AccountId, long CategoryId, string Name, decimal Amount, 
    DateOnly TransactionDate, string? Description);