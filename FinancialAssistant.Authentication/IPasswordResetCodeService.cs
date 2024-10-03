namespace FinancialAssistant.Authentication;

public interface IPasswordResetCodeService
{
    Task<string> GeneratePasswordResetCode(string email, CancellationToken cancellationToken);

    Task<bool> AuthenticPasswordResetCode(long userId, string code, CancellationToken cancellationToken);
}