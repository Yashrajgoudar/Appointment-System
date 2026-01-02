namespace AuthService.Application.Common.Interfaces
{
    public interface IPasswordService
    {
        (byte[] hash, byte[] salt) HashPassword(string password);
        bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
        bool ValidatePasswordStrength(string password);
    }
}
