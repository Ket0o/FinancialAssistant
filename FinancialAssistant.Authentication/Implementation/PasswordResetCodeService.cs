using System.Text;
using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.Repository;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace FinancialAssistant.Authentication.Implementation;

public class PasswordResetCodeService : IPasswordResetCodeService
{
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const string ErrorEmailMessage = "Error email";
    
    private readonly IPasswordResetCodeRepository _passwordResetCodeRepository;
    private readonly IUserRepository _userRepository;

    public PasswordResetCodeService(
        IPasswordResetCodeRepository passwordResetCodeRepository, 
        IUserRepository userRepository)
    {
        _passwordResetCodeRepository = passwordResetCodeRepository;
        _userRepository = userRepository;
    }

    public async Task<OneOf<Success<string>, Error<string>>> GeneratePasswordResetCode(string email, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetAsync(userFromDb => EF.Functions.ILike(userFromDb.Email, email), cancellationToken)
            is not { } user)
            return new Error<string>(ErrorEmailMessage);

        string code;
        if (await _passwordResetCodeRepository.GetAsync(code => code.UserId == user.Id, cancellationToken) is not
            { } resetCode)
        {
            code = await GenerateCodeUntilItUnique(cancellationToken);
            await _passwordResetCodeRepository.AddAsync(new PasswordResetCode
                {ExpirationDate = DateTime.Now.AddDays(1), ResetCode = code, UserId = user.Id});
            return new Success<string>(code);
        }
        
        if (resetCode.ExpirationDate > DateTime.Now)
            return new Success<string>(resetCode.ResetCode);

        code = await GenerateCodeUntilItUnique(cancellationToken); 
        await _passwordResetCodeRepository.AddAsync(new PasswordResetCode
            {ExpirationDate = DateTime.Now.AddDays(1), ResetCode = code, UserId = user.Id});
        return new Success<string>(code);
    }

    public async Task<bool> AuthenticPasswordResetCode(long userId, string code, CancellationToken cancellationToken)
    {
        if (await _passwordResetCodeRepository.GetAsync(
                resetCode => resetCode.UserId == userId && EF.Functions.ILike(resetCode.ResetCode, code),
                cancellationToken) is not { } resetCodeFromDb)
            return false;

        await _passwordResetCodeRepository.DeleteAsync(resetCodeFromDb);
        return true;
    }

    private static string GenerateCode()
    {
        StringBuilder codeBuilder = new();

        for (var i = 0; i < 5; i++)
            codeBuilder.Append(Characters[Random.Shared.Next(Characters.Length)]);

        return codeBuilder.ToString();
    }

    private async Task<string> GenerateCodeUntilItUnique(CancellationToken cancellationToken)
    {
        var code = GenerateCode();
        while (await _passwordResetCodeRepository.IsExistingCode(code, cancellationToken))
            code = GenerateCode();
        return code;
    }
}