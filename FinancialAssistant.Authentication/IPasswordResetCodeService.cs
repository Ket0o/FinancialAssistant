using OneOf;
using OneOf.Types;

namespace FinancialAssistant.Authentication;

public interface IPasswordResetCodeService
{
    Task<OneOf<Success<string>, Error<string>>> GeneratePasswordResetCode(string email,
        CancellationToken cancellationToken);

    Task<bool> AuthenticPasswordResetCode(long userId, string code, CancellationToken cancellationToken);
}