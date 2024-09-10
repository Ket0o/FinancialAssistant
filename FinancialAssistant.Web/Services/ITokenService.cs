using FinancialAssistant.DataTransfer.User;

namespace FinancialAssistant.Web.Services;

public interface ITokenService
{
    string GenerateJwtToken(InputUserDto user, out DateTime expires);

    string GenerateRefreshToken();
}