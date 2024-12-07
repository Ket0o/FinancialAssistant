using FinancialAssistant.DataAccess.Model;

namespace FinancialAssistant.Repository;

public interface IPasswordResetCodeRepository : IRepository<PasswordResetCode>
{
    Task<bool> IsExistingCode(string code, CancellationToken cancellationToken);
}