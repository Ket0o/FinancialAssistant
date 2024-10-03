namespace FinancialAssistant.Authentication;

public interface IPasswordService
{
    string HashPassword(string password, out byte[] salt);
    bool VerifyPassword(string password, string hashedPassword, byte[] salt);
}