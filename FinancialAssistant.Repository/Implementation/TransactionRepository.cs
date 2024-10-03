using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;

namespace FinancialAssistant.Repository.Implementation;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(DataContext dataContext) : base(dataContext)
    {
        
    }    
}