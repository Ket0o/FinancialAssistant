namespace FinancialAssistant.Authentication;

public interface IUserIdentityService
{
    string? GetEmail();

    long GetUserId();

    string? GetUserName();
}