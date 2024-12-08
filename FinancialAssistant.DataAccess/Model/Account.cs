namespace FinancialAssistant.DataAccess.Model;

public class Account : BaseEntity, IContainsUserId
{
    public User User { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public Boolean IsDefault { get; set; }
}