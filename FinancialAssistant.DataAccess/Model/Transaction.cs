namespace FinancialAssistant.DataAccess.Model;

public class Transaction : BaseEntity
{
    public long AccountId { get; set; }
    public long CategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateOnly TransactionDate { get; set; }
    public string Description { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
}