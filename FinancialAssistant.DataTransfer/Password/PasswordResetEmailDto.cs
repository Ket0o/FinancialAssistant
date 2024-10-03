namespace FinancialAssistant.DataTransfer.Password;

public record PasswordResetEmailDto(string Email, string Code);