namespace FinancialAssistant.DataAccess.Model;

public class User : BaseEntity
{
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public byte[] Salt { get; set; }
    public string Email { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset? TokenCreated { get; set; }
    public DateTimeOffset? TokenExpires { get; set; }
}