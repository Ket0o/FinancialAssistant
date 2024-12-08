using FinancialAssistant.DataTransfer.Password;
using FinancialAssistant.DataTransfer.Token;
using FinancialAssistant.DataTransfer.User;
using OneOf;
using OneOf.Types;

namespace FinancialAssistant.Web.Services;

public interface IUserService
{
    Task<OneOf<Success<string>, Error<string>>> CreateUser(CreateUserDto createUser,
        CancellationToken cancellationToken);
    Task<OneOf<TokensDto, Error<string>>> AuthenticationUser(AuthorizationUserDto user,
        CancellationToken cancellationToken);
    Task<OneOf<Success<string>, Error<string>>> ForgotPassword(string email, CancellationToken cancellationToken);
    Task<OneOf<Success<string>, Error<string>>> ChangeUserPassword(ChangePasswordDto changePassword,
        CancellationToken cancellationToken);
}