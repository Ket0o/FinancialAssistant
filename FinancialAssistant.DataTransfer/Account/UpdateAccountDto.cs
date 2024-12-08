namespace FinancialAssistant.DataTransfer.Account;

public record UpdateAccountDto(long Id, string Name, bool IsDefault);