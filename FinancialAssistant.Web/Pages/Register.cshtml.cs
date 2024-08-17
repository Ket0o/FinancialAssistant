using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinancialAssistant.Web.Pages
{
    public class RegisterModel : PageModel
    {
        public string? ErrorMessage { get; set; }
        public void OnGet()
        {
        }
    }
}
