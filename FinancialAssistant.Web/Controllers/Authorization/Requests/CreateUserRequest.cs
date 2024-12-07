namespace FinancialAssistant.Web.Controllers.Authorization.Requests;

public record CreateUserRequest(string UserName, string Email, string Password);