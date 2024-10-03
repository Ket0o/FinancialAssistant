using FinancialAssistant.DataTransfer.Password;
using FinancialAssistant.DataTransfer.User;

namespace FinancialAssistant.EmailService
{
    public interface IEmailService
    {
        Task SendEmail(PasswordResetEmailDto passwordResetEmail, CancellationToken cancellationToken);
    }
}