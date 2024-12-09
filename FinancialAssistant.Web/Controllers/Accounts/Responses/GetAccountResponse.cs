namespace FinancialAssistant.Web.Controllers.Accounts.Responses;

public record GetAccountResponse(long Id, string Name, decimal Balance, bool IsDefault);