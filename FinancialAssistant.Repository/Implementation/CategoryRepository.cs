using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;

namespace FinancialAssistant.Repository.Implementation;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(DataContext dataContext) : base(dataContext)
    {
        
    }
}