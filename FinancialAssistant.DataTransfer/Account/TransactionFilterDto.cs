namespace FinancialAssistant.DataTransfer.Account;

public record TransactionFilterDto(int? AccountId, int? CategoryId, string? YearMonth, string? SortBy, 
    bool? SortDescending);