namespace FinancialAssistant.DataTransfer.Transaction
{
    public record UpdateTransactionDto(long Id, long AccountId, long CategoryId, decimal Amount, 
        DateOnly TransactionDate, string Description);
}
