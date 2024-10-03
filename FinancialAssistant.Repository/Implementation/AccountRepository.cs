using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;

namespace FinancialAssistant.Repository.Implementation;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(DataContext dataContext) : base(dataContext)
    {
        
    }
}