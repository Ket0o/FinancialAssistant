namespace FinancialAssistant.Web.Controllers.Authorization.Requests;

public record LoginUserRequest(string Email, string Password);