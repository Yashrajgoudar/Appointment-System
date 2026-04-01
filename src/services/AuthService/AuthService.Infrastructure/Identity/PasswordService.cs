using AuthService.Application.Common.Interfaces;
using System.Security.Cryptography;

namespace AuthService.Infrastructure.Identity
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 128;
        private const int HashSize = 64;
        private const int Iterations = 100000;

        public (byte[] hash, byte[] salt) HashPassword(string password)
        {
            // Generate random salt
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Generate hash using PBKDF2 with HMAC-SHA512
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA512);

            var hash = pbkdf2.GetBytes(HashSize);

            return (hash, salt);
        }

        public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            // Generate hash with the stored salt
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                storedSalt,
                Iterations,
                HashAlgorithmName.SHA512);

            var computedHash = pbkdf2.GetBytes(HashSize);

            // Compare hashes using constant-time comparison
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }

        public bool ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            // Require at least 3 out of 4 criteria
            int criteriaMet = (hasUpper ? 1 : 0) + (hasLower ? 1 : 0) +
                             (hasDigit ? 1 : 0) + (hasSpecial ? 1 : 0);

            return criteriaMet >= 3;
        }
    }
}
