using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.UserIdentity;

namespace FinancialAssistant.Repository.Implementation;

public class CategoryRepository : RepositoryForContainsUserId<Category>, ICategoryRepository
{
    public CategoryRepository(DataContext dataContext, IUserIdentityService identityService) 
        : base(dataContext, identityService)
    {
        
    }
}