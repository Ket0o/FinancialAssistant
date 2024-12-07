using FinancialAssistant.DataAccess;
using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.UserIdentity;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistant.Repository.Implementation;

public class PasswordResetCodeRepository : Repository<PasswordResetCode>, IPasswordResetCodeRepository
{
    private readonly DataContext _dataContext;
    
    public PasswordResetCodeRepository(DataContext dataContext) 
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> IsExistingCode(string code, CancellationToken cancellationToken)
        => await _dataContext.PasswordResetCodes
            .AsNoTracking()
            .AsQueryable()
            .CountAsync(resetCode => EF.Functions.ILike(resetCode.ResetCode, code), cancellationToken) > 0;
}