using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FinancialAssistant.Authentication;
using FinancialAssistant.DataTransfer.Password;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinancialAssistant.Web.Pages;

public class EmailAccess : PageModel
{
    private readonly IUserService _userService;

    public EmailAccess(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty(SupportsGet = true)] 
    public AccessEmailInputDto Input { get; set; } = new();
    
    public void OnGet(string email)
    {
        if (email is not {})
            return;
        Input.Email = email;
    }

    public async Task<ActionResult> OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        if (!await _userService.ChangeUserPassword(new ChangePasswordDto(Input.Email, Input.NewPassword, Input.Code),
                Input.CancellationToken))
            return Page();

        return RedirectToPage("/Login");
    }
    
    public class AccessEmailInputDto
    {
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Новый пароль")]
        [PasswordPropertyText]
        public string NewPassword { get; set; }
        
        [Required]
        [DisplayName("Подтверждение пароля")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Пароли не совпадают")]
        public string AccessNewPassword { get; set; }
        
        [Required]
        [DisplayName("Код с почты")]
        public string Code { get; set; }
        
        public CancellationToken CancellationToken { get; set; }
    }
}