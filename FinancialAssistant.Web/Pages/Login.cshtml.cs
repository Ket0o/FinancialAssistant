using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FinancialAssistant.Authentication;
using FinancialAssistant.DataTransfer.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinancialAssistant.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public LoginInputDto Input { get; set; }

        public void OnGet()
        {

        }
        
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var tokens = await _userService.AuthenticationUser(new AuthorizationUserDto(Input.Email, Input.Password),
                Input.CancellationToken);

            if (string.IsNullOrWhiteSpace(tokens.AccessToken) || string.IsNullOrWhiteSpace(tokens.RefreshToken))
                return Page();
            
            HttpContext.Session.SetString("Token", tokens.AccessToken);
            return RedirectToPage("Index");
        }
    }

    public class LoginInputDto
    {
        [EmailAddress(ErrorMessage = "Некорректная почта!")]
        [Required]
        [DisplayName("Почта")]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Пароль")]
        [PasswordPropertyText]
        public string Password { get; set; }
        
        public CancellationToken CancellationToken { get; set; }
    }
}
