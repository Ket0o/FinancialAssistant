namespace FinancialAssistant.DataTransfer.Transaction;

public record GetTransactionDto(long Id, string Name, long AccountId, long CategoryId, decimal Amount, 
        DateOnly TransactionDate, string Description, DateTime CreatedAt, string AccountName, string CategoryName, 
        string CategoryColor);
