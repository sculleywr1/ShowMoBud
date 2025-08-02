using Microsoft.Extensions.Configuration;
using ShowMoBudAPI.Services;
using ShowMoBudAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShowMoBudAPI.Tests.Services
{
    public class JwtServiceTest
    {

        private JwtService _jwtService;

        public JwtServiceTest()
        {
            // Initialize the JwtService with in-memory configuration settings
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "JwtSettings:SecretKey", "TestSecretKey123456789012345678901234567890123456789012345678901234567890" },
                { "JwtSettings:Issuer", "TestIssuer" },
                { "JwtSettings:Audience", "TestAudience" },
                { "JwtSettings:ExpirationMinutes", "60" }
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            _jwtService = new JwtService(config);
        }

        [Fact]
        public void JwtService_ShouldReturnValidJwtResponse()
        {
            //arrange
            var username = "testuser";
            var roles = new Role
            {
                RoleId = 3,
                RoleName = "Premium"
            };

            // Act
            var result = _jwtService.GenerateToken(username, roles);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token), "JWT token should not be null or empty.");
            Assert.Equal(username, result.Username);
            Assert.True(result.Expiration > DateTime.UtcNow, "JWT token expiration should be in the future.");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.Token);

            Assert.Equal("TestIssuer", token.Issuer);
            Assert.Contains("TestAudience", token.Audiences);
            Assert.Contains(token.Claims, c => c.Type == "sub" && c.Value == username);
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Premium");
        }

    }
}
