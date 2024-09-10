using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FinancialAssistant.DataTransfer.User;
using FinancialAssistant.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinancialAssistant.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty(SupportsGet = true)]
        public RegisterInputDto Input { get; set; }
        
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
                return Page();

            await _userService.CreateUser(new CreateUserDto(Input.UserName, Input.Email, Input.Password),
                Input.CancellationToken);
            
            return RedirectToPage("Login");
        }
    }

    public class RegisterInputDto
    {
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Имя пользователя")]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Введите корректную почту.")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Почта")]
        public string Email { get; set; }
        
        [Required]
        [DisplayName("Пароль")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [DisplayName("Подтверждение пароля")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public CancellationToken CancellationToken { get; set; }
    }
}
