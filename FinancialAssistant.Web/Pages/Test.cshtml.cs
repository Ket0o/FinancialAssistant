using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FinancialAssistant.Web.Pages
{
    public class TestModel : PageModel
    {
        public void OnGet(int number)
        {
            Square = number * number;
        }

        public int Square { get; set; }
    }
}
