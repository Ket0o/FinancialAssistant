using FinancialAssistant.DataTransfer.Token;
using FinancialAssistant.DataTransfer.User;

namespace FinancialAssistant.Web.Services;

public interface IUserService
{
    Task CreateUser(CreateUserDto createUser, CancellationToken cancellationToken);

    Task<TokensDto> AuthorizationUser(AuthorizationUserDto user, CancellationToken cancellationToken);
}