namespace FinancialAssistant.Web.Controllers.Authorization.Responses;

public record LoginUserResponse(string AccessToken, string RefreshToken);