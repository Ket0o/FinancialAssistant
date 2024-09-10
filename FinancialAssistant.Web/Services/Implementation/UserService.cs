using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Token;
using FinancialAssistant.DataTransfer.User;
using FinancialAssistant.Repository;

namespace FinancialAssistant.Web.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository userRepository, 
        IPasswordService passwordService, 
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
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

    public async Task<TokensDto> AuthorizationUser(AuthorizationUserDto authorizationUser, CancellationToken cancellationToken)
    {
        if (await _userRepository
                .GetAsync(user => user.Email == authorizationUser.Email, cancellationToken) is not { } user)
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
}