using FinancialAssistant.Authentication;
using FinancialAssistant.UserIdentity;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistant.Web.ViewComponents;

public class UserInfoViewComponent : ViewComponent
{
    private readonly IUserIdentityService _identityService;

    public UserInfoViewComponent(IUserIdentityService identityService)
    {
        _identityService = identityService;
    }

    public IViewComponentResult Invoke()
    {
        var userName = _identityService.GetUserName()!;
        var shortUserName = userName.Length >= 2 ? userName[..2] : userName;

        return View(new { UserName = userName, ShortUserName = shortUserName });
    }
}