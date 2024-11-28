using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Category;
using FinancialAssistant.Repository;

namespace FinancialAssistant.Web.Services.Implementation;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(
        ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task AddCategory(CreateCategoryDto createCategory, CancellationToken cancellationToken)
    {
        await _categoryRepository.AddAsync(new Category
        {
            CreatedAt = DateTime.Now,
            IsIncome = createCategory.IsIncome,
            Name = createCategory.Name,
            Color = createCategory.Color
        });
    }

    public async Task ChangeCategory(UpdateCategoryDto updateCategory, CancellationToken cancellationToken)
    {
        if (await _categoryRepository.GetAsync(category => category.Id == updateCategory.Id, cancellationToken) is not
            { } category)
            return;

        category.Name = updateCategory.Name;
        category.IsIncome = updateCategory.IsIncome;
        category.Color = updateCategory.Color;
        await _categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteCategory(long id, CancellationToken cancellationToken)
    {
        if (await _categoryRepository.GetAsync(category => category.Id == id, cancellationToken) is not {} category)
            return;

        await _categoryRepository.DeleteAsync(category);
    }
}