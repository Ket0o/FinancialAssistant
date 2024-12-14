namespace FinancialAssistant.Web.Controllers.Transactions.Requests;

public record GetTransactionRequest(int? AccountId, int? CategoryId, string? YearMonth, string? SortBy);