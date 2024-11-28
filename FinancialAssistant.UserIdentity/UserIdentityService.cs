using FinancialAssistant.DataTransfer.User;
using Microsoft.AspNetCore.Http;

namespace FinancialAssistant.UserIdentity;

public class UserIdentityService : IUserIdentityService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserIdentityService(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

    public string? GetEmail()
        => _contextAccessor.HttpContext?.User.FindFirst(nameof(UserClaimsDto.Email)) is { } email 
            ? email.Value 
            : default;

    public long GetUserId()
    {
        if (_contextAccessor.HttpContext?.User.Claims.First().Value is not { } userId)
            return -1;
        
        return long.Parse(userId);
    }

    public string? GetUserName()
        => _contextAccessor.HttpContext?.User.FindFirst(nameof(UserClaimsDto.Name)) is { } name 
            ? name.Value 
            : default;
}