using System.Security.Cryptography;
using System.Text;

namespace ShowMoBudAPI.Services
{
    public class EncryptionService
    {
        public byte[] GenerateSalt(int size = 25)
        {
            var salt = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        // Hash the password using PBKDF2 (Rfc2898) with SHA-512.
        // The password is encoded to bytes and used via a ReadOnlySpan<byte>;
        // the temporary password byte array is cleared after use to reduce memory exposure.
        public Byte[] HashPassword(string password, byte[] salt)
        {
            const int iterations = 100_000; // Increase to slow brute-force (tunable)
            const int derivedKeyLength = 64; // 64 bytes = 512 bits (SHA-512 sized)

            // Encode password to UTF-8 bytes (temporary array)
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            try
            {
                // Use a ReadOnlySpan<byte> over the password bytes for the Rfc2898DeriveBytes constructor
                ReadOnlySpan<byte> passwordSpan = passwordBytes;

                // Create PBKDF2 instance using SHA-512 and derive the key
                using var pbkdf2 = new Rfc2898DeriveBytes(passwordSpan, salt, iterations, HashAlgorithmName.SHA512);
                return pbkdf2.GetBytes(derivedKeyLength);
            }
            finally
            {
                // Clear sensitive data from the temporary array
                CryptographicOperations.ZeroMemory(passwordBytes);
            }
        }
    }
}
