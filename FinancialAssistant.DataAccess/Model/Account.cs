namespace FinancialAssistant.DataAccess.Model;

public class Account : BaseEntity
{
    public long UserId { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}