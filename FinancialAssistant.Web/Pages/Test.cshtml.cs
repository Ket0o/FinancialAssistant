using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace FinancialAssistant.Web.Pages
{
    [Authorize]
    public class TestModel : PageModel
    {
        public void OnGet(int number)
        {
            Square = number * number;
        }

        public int Square { get; set; }
    }
}
