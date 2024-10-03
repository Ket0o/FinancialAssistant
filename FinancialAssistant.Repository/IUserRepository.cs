using FinancialAssistant.DataAccess.Model;

namespace FinancialAssistant.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<bool> ItExistingUser(string email, CancellationToken cancellationToken);
}