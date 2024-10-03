namespace FinancialAssistant.DataAccess.Model;

public class PasswordResetCode : BaseEntity
{
    public long UserId { get; set; }
    public string ResetCode { get; set; }
    public DateTime ExpirationDate { get; set; } 
}