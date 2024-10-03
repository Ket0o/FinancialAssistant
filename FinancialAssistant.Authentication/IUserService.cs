using FinancialAssistant.DataTransfer.Password;
using FinancialAssistant.DataTransfer.Token;
using FinancialAssistant.DataTransfer.User;

namespace FinancialAssistant.Authentication;

public interface IUserService
{
    Task CreateUser(CreateUserDto createUser, CancellationToken cancellationToken);
    Task<TokensDto> AuthenticationUser(AuthorizationUserDto user, CancellationToken cancellationToken);
    Task<bool> ItExistingUser(string email, CancellationToken cancellationToken);
    Task<bool> ChangeUserPassword(ChangePasswordDto changePassword, CancellationToken cancellationToken);
    string? GetUserName();
    long? GetUserId();
}