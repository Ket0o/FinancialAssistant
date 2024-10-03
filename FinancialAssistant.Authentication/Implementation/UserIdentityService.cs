using FinancialAssistant.DataTransfer.User;
using Microsoft.AspNetCore.Http;

namespace FinancialAssistant.Authentication.Implementation;

public class UserIdentityService : IUserIdentityService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserIdentityService(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

    public string? GetEmail()
        => _contextAccessor.HttpContext?.User.FindFirst(nameof(UserClaimsDto.Email)) is { } email 
            ? email.Value 
            : default;

    public long GetUserId()
        => _contextAccessor.HttpContext?.User.FindFirst(nameof(UserClaimsDto.Sub)) is { } userId 
            ? long.TryParse(userId.Value, out var result) 
                ? result 
                : default 
            : default;

    public string? GetUserName()
        => _contextAccessor.HttpContext?.User.FindFirst(nameof(UserClaimsDto.Name)) is { } name 
            ? name.Value 
            : default;
}