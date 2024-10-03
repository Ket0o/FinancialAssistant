using System.Security.Cryptography;
using System.Text;

namespace FinancialAssistant.Authentication.Implementation;

public class PasswordService : IPasswordService
{
    private const int KeySize = 64;
    private const int Iterations = 400000;
    private static readonly HashAlgorithmName AlgorithmName = HashAlgorithmName.SHA512;

    public string HashPassword(string password, out byte[] salt)
    {
        salt = GenerateSalt();
        var hashPassword = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            AlgorithmName, 
            KeySize
        );

        return Convert.ToHexString(hashPassword);
    }

    public bool VerifyPassword(string password, string hashedPassword, byte[] salt)
    {
        var inputHashPassword 
            = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, AlgorithmName, KeySize);

        return CryptographicOperations.FixedTimeEquals(inputHashPassword, 
            Convert.FromHexString(hashedPassword));
    }

    private static byte[] GenerateSalt()
    {
        var salt = new byte[KeySize];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(salt);
        return salt;
    }
}