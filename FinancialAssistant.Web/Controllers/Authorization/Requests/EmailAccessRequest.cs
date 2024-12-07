namespace FinancialAssistant.Web.Controllers.Authorization.Requests;

public record EmailAccessRequest(string Email, string NewPassword, string Code);