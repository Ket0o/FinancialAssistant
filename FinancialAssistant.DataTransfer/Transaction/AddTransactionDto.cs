namespace FinancialAssistant.DataTransfer.Transaction
{
    public record AddTransactionDto(long AccountId, long CategoryId, string Name, decimal Amount, 
        DateOnly TransactionDate, string? Description);
}
