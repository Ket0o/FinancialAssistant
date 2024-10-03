using FinancialAssistant.DataTransfer.User;

namespace FinancialAssistant.Authentication;

public interface ITokenService
{
    string GenerateJwtToken(InputUserDto user, out DateTime expires);
    string GenerateRefreshToken();
}