namespace FinancialAssistant.DataTransfer.Category;

public record CreateCategoryDto(string Name, bool IsIncome, string Color);