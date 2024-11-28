namespace FinancialAssistant.DataTransfer.Category;

public record UpdateCategoryDto(long Id, string Name, bool IsIncome, string Color);