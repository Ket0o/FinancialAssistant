namespace FinancialAssistant.Web.Services;

public interface IPasswordService
{
    string HashPassword(string password, out byte[] salt);

    bool VerifyPassword(string password, string hashedPassword, byte[] salt);
}