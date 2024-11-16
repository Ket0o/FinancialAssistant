namespace FinancialAssistant.DataTransfer.Transaction;

public record GetTransactionDto(long Id, long AccountId, long CategoryId, decimal Amount, 
        DateOnly TransactionDate, string Description, DateTime CreatedAt);
