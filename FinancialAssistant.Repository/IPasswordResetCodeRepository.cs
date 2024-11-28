using FinancialAssistant.DataAccess.Model;

namespace FinancialAssistant.Repository;

public interface IPasswordResetCodeRepository : IRepositoryForContainsUserId<PasswordResetCode>
{
    Task<bool> IsExistingCode(string code, CancellationToken cancellationToken);
}