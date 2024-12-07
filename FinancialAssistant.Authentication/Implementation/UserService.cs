using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Password;
using FinancialAssistant.DataTransfer.Token;
using FinancialAssistant.DataTransfer.User;
using FinancialAssistant.Repository;
using FinancialAssistant.UserIdentity;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace FinancialAssistant.Authentication.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IPasswordResetCodeService _passwordResetCodeService;

    public UserService(IUserRepository userRepository, 
        IPasswordService passwordService, 
        ITokenService tokenService, 
        IPasswordResetCodeService passwordResetCodeService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _passwordResetCodeService = passwordResetCodeService;
    }

    public async Task<OneOf<Success<string>, Error<string>>> CreateUser(CreateUserDto createUser, CancellationToken cancellationToken)
    {
        if (await _userRepository
                .GetAsync(user => user.UserName == createUser.UserName || user.Email == createUser.Email, 
                    cancellationToken) is {})
            return new Error<string>("Пользователь уже существует.");
        
        User user = new()
        {
            PasswordHash = _passwordService.HashPassword(createUser.Password, out var salt),
            Salt = salt,
            UserName = createUser.UserName,
            Email = createUser.Email,
            CreatedOn = DateTime.Now
        };

        await _userRepository.AddAsync(user);
        return new Success<string>("Пользователь успешно зарегистрирован.");
    }

    public async Task<OneOf<TokensDto, Error<string>>> AuthenticationUser(AuthorizationUserDto authorizationUser, CancellationToken cancellationToken)
    {
        if (await _userRepository
                .GetAsync(userFromDb => userFromDb.Email == authorizationUser.Email, cancellationToken) is not { } user)
            return new Error<string>("Пользователь не зарегистрирован.");

        if (!_passwordService.VerifyPassword(authorizationUser.Password, user.PasswordHash, user.Salt))
            return new Error<string>("Не верный пароль.");

        var accessToken =
            _tokenService.GenerateJwtToken(new InputUserDto(user.Email, user.Id, user.UserName), out var expires);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.TokenCreated = DateTime.Now;
        user.TokenExpires = expires;
        await _userRepository.UpdateAsync(user);

        return new TokensDto(accessToken, refreshToken);
    }

    public async Task<OneOf<Success<string>, Error<string>>> ForgotPassword(string email, CancellationToken cancellationToken)
    {
        if (!await ItExistingUser(email, cancellationToken))
            return new Error<string>("Пользователь не зарегистрирован.");

        var code = await _passwordResetCodeService.GeneratePasswordResetCode(email, cancellationToken);

        return code.IsT0 ? code.AsT0 : code.AsT1;
    }
    
    public async Task<OneOf<Success<string>, Error<string>>> ChangeUserPassword(ChangePasswordDto changePassword, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetAsync(userFromDb => EF.Functions.ILike(userFromDb.Email, changePassword.Email),
                cancellationToken) is not { } user)
            return new Error<string>("Пользователь не зарегистрирован.");
        
        if (!await _passwordResetCodeService.AuthenticPasswordResetCode(user.Id, changePassword.Code, cancellationToken))
            return new Error<string>("Неверный код.");

        user.PasswordHash = _passwordService.HashPassword(changePassword.NewPassword, out var salt);
        user.Salt = salt;

        await _userRepository.UpdateAsync(user);
        return new Success<string>("Пароль обновлен.");
    }
    
    private async Task<bool> ItExistingUser(string email, CancellationToken cancellationToken)
        => await _userRepository.ItExistingUser(email, cancellationToken);
}