namespace FinancialAssistant.DataAccess.Model;

public class Category : BaseEntity, IContainsUserId
{
    public User User { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; }
    public bool IsIncome { get; set; }
    public string Color { get; set; }
    public DateTime CreatedAt { get; set; } 
}