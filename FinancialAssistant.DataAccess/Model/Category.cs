namespace FinancialAssistant.DataAccess.Model;

public class Category : BaseEntity
{
    public long? UserId { get; set; }
    public string Name { get; set; }
    public bool IsIncome { get; set; }
    public DateTime CreatedAt { get; set; } 
}