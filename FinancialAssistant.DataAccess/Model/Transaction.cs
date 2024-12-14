namespace FinancialAssistant.DataAccess.Model;

public class Transaction : BaseEntity, IContainsUserId
{
    public string Name { get; set; } = String.Empty;
    public Account Account { get; set; }
    public long AccountId { get; set; }
    public Category Category { get; set; }
    public long CategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateOnly TransactionDate { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public User User { get; set; }
    public long UserId { get; set; }
}