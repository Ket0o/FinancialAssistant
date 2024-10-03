using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FinancialAssistant.Authentication;
using FinancialAssistant.DataTransfer.Password;
using FinancialAssistant.EmailService;
using FinancialAssistant.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinancialAssistant.Web.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IPasswordResetCodeService _passwordResetCodeService;
        
        public ForgotPasswordModel(IUserService userService, 
            IEmailService emailService, 
            IPasswordResetCodeService passwordResetCodeService)
        {
            _userService = userService;
            _emailService = emailService;
            _passwordResetCodeService = passwordResetCodeService;
        }

        [Required]
        [EmailAddress(ErrorMessage = "Введите корректную почту.")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Почта")]
        [BindProperty]
        public string Email { get; set; }
        
        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPost(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return Page();

            if (!await _userService.ItExistingUser(Email, cancellationToken))
                return Page();

            var code = await _passwordResetCodeService.GeneratePasswordResetCode(Email, cancellationToken);
            await _emailService.SendEmail(new PasswordResetEmailDto(Email, code), cancellationToken);

            return RedirectToPage("/EmailAccess", new {email = Email});
    }
    }
}
