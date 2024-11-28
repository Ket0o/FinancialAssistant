using FinancialAssistant.DataTransfer.Category;

namespace FinancialAssistant.Web.Services;

public interface ICategoryService
{
    Task AddCategory(CreateCategoryDto createCategory, CancellationToken cancellationToken);

    Task ChangeCategory(UpdateCategoryDto updateCategory, CancellationToken cancellationToken);

    Task DeleteCategory(long id, CancellationToken cancellationToken);
}