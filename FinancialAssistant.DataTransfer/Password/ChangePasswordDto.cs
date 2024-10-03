namespace FinancialAssistant.DataTransfer.Password;

public record ChangePasswordDto(string Email, string NewPassword, string Code);