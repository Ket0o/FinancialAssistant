using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;

namespace FinancialAssistant.Repository.Implementation;

public class UserRepository : Repository<User>, IUserRepository 
{
    public UserRepository(DataContext dbContext) : base(dbContext)
    {
    }
}