using System.Security.Cryptography;
using System.Text;

namespace ShowMoBudAPI.Services
{
    public class EncryptionService
    {
        public byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        public Byte[] HashPassword(string password, byte[] salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combined = passwordBytes.Concat(salt).ToArray();

            using var sha = SHA512.Create();
            return sha.ComputeHash(combined);
        }
    }
}
