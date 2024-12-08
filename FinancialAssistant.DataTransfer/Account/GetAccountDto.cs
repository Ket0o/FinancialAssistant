namespace FinancialAssistant.DataTransfer.Account;

public record GetAccountDto(long Id, string Name, decimal Balance, bool IsDefault);