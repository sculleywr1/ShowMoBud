using ShowMoBudAPI.Services;

namespace ShowMoBudAPI.Tests.Services
{
    public class EncryptionServiceTest
    {

        private readonly EncryptionService _encrypt = new EncryptionService();

        [Fact]
        public void GenerateSalt_ShouldReturnByteArrayOfCorrectLength()
        {
            // Arrange
            int expectedLength = 16; // Default salt length
            // Act
            var salt = _encrypt.GenerateSalt();
            // Assert
            Assert.NotNull(salt);
            Assert.Equal(expectedLength, salt.Length);
        }

        [Fact]
        public void HashPassword_ShouldReturnConsistentHash()
        {
            // Arrange
            string password = "TestPassword";
            byte[] salt = _encrypt.GenerateSalt();
            // Act
            var hash1 = _encrypt.HashPassword(password, salt);
            var hash2 = _encrypt.HashPassword(password, salt);
            // Assert
            Assert.Equal(hash1, hash2);
        }

    }
}
