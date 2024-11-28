namespace FinancialAssistant.UserIdentity;

public interface IUserIdentityService
{
    string? GetEmail();

    long GetUserId();

    string? GetUserName();
}