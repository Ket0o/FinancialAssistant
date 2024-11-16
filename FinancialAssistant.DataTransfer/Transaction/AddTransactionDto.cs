namespace FinancialAssistant.DataTransfer.Transaction
{
    public record AddTransactionDto(long AccountId, long CategoryId, decimal Amount, 
        DateOnly TransactionDate, string Description);
}
