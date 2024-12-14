namespace FinancialAssistant.DataTransfer.Transaction
{
    public record UpdateTransactionDto(long Id, long AccountId, string Name, long CategoryId, decimal Amount, 
        DateOnly TransactionDate, string? Description);
}
