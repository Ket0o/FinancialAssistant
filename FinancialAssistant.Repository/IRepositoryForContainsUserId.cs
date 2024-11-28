using FinancialAssistant.DataAccess.Model;

namespace FinancialAssistant.Repository;

public interface IRepositoryForContainsUserId<T> : IRepository<T> where T : class, IContainsUserId
{
    
}