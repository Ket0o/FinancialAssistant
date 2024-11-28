using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Password;
using FinancialAssistant.DataTransfer.Token;
using FinancialAssistant.DataTransfer.User;
using FinancialAssistant.Repository;
using FinancialAssistant.UserIdentity;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistant.Authentication.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IPasswordResetCodeService _passwordResetCodeService;
    private readonly IUserIdentityService _identityService;

    public UserService(IUserRepository userRepository, 
        IPasswordService passwordService, 
        ITokenService tokenService, 
        IPasswordResetCodeService passwordResetCodeService, 
        IUserIdentityService identityService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _passwordResetCodeService = passwordResetCodeService;
        _identityService = identityService;
    }

    public async Task CreateUser(CreateUserDto createUser, CancellationToken cancellationToken)
    {
        if (await _userRepository
                .GetAsync(user => user.UserName == createUser.UserName || user.Email == createUser.Email, 
                    cancellationToken) is {})
            return;
        
        User user = new()
        {
            PasswordHash = _passwordService.HashPassword(createUser.Password, out var salt),
            Salt = salt,
            UserName = createUser.UserName,
            Email = createUser.Email,
            CreatedOn = DateTime.Now
        };

        await _userRepository.AddAsync(user);
    }

    public async Task<TokensDto> AuthenticationUser(AuthorizationUserDto authorizationUser, CancellationToken cancellationToken)
    {
        if (await _userRepository
                .GetAsync(userFromDb => userFromDb.Email == authorizationUser.Email, cancellationToken) is not { } user)
            return new TokensDto(string.Empty, string.Empty);

        if (!_passwordService.VerifyPassword(authorizationUser.Password, user.PasswordHash, user.Salt))
            return new TokensDto(string.Empty, string.Empty);

        var accessToken =
            _tokenService.GenerateJwtToken(new InputUserDto(user.Email, user.Id, user.UserName), out var expires);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.TokenCreated = DateTime.Now;
        user.TokenExpires = expires;
        await _userRepository.UpdateAsync(user);

        return new TokensDto(accessToken, refreshToken);
    }
    
    public async Task<bool> ItExistingUser(string email, CancellationToken cancellationToken)
        => await _userRepository.ItExistingUser(email, cancellationToken);

    public async Task<bool> ChangeUserPassword(ChangePasswordDto changePassword, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetAsync(userFromDb => EF.Functions.ILike(userFromDb.Email, changePassword.Email),
                cancellationToken) is not { } user)
            return false;
        
        if (!await _passwordResetCodeService.AuthenticPasswordResetCode(user.Id, changePassword.Code, cancellationToken))
            return false;

        user.PasswordHash = _passwordService.HashPassword(changePassword.NewPassword, out var salt);
        user.Salt = salt;

        await _userRepository.UpdateAsync(user);
        return true;
    }

    public string? GetUserName() => _identityService.GetUserName();
    public long? GetUserId() => _identityService.GetUserId();
}