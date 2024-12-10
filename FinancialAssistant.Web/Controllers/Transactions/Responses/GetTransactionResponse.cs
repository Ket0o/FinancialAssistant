namespace FinancialAssistant.Web.Controllers.Transactions.Responses;

public record GetTransactionResponse(long Id, string Name, long AccountId, long CategoryId, decimal Amount, 
    DateOnly TransactionDate, string Description, DateTime CreatedAt, string AccountName, string CategoryName, 
    string CategoryColor);