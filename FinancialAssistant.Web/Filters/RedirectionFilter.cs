using FinancialAssistant.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinancialAssistant.Web.Filters;

public class RedirectionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
            context.Result = new RedirectToActionResult(nameof(LoginModel.OnPost), "Login", null);
    }
}