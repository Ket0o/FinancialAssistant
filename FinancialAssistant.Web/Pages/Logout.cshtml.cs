using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinancialAssistant.Web.Pages;

public class Logout : PageModel
{
    public ActionResult OnGet()
    {
        Response.Cookies.Delete(".AspNetCore.Session");
        Response.Cookies.Delete(".AspNetCore.Antiforgery.9GoFmqeO83E");
        return RedirectToPage("/Login");
    }
}