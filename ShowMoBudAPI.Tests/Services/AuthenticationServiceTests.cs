using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using ShowMoBudAPI.Contexts;
using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Models;
using ShowMoBudAPI.Services;
using ShowMoBudAPI.Services.Interfaces;

namespace ShowMoBudAPI.Tests.Services
{
    public class AuthenticationServiceTests
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly Mock<ShowMoBudContext> _mockDbContext;
        private readonly Mock<IEncryptionService> _mockEncryptionService;
        private readonly Mock<IJwtService> _mockJwtService;

        public AuthenticationServiceTests()
        {

            _mockDbContext = new Mock<ShowMoBudContext>(new DbContextOptions<ShowMoBudContext>());
            _mockEncryptionService = new Mock<IEncryptionService>();
            _mockJwtService = new Mock<IJwtService>();

            //setup minimal behavior for the mocks
            _mockEncryptionService
                .Setup(es => es.GenerateSalt())
                .Returns(new byte[16]);

            _mockEncryptionService.Setup(es => es.HashPassword(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Returns(new byte[64]);

            _mockJwtService
                .Setup(js => js.GenerateToken(It.IsAny<string>(), It.IsAny<Role>()))
                .Returns(new JwtResponse
                {
                    Token = "mocked_token",
                    Username = "testuser",
                    Expiration = DateTime.UtcNow.AddHours(1)
                }
            );

            _authenticationService = new AuthenticationService(
                _mockDbContext.Object,
                _mockEncryptionService.Object,
                _mockJwtService.Object
            );
        }

        [Fact]
        public void Register_ShouldCreateUser_AndReturnToken()
        {
            // Arrange
            var registrationDto = new RegistrationDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "TestPassword123!",
                IdVerificationStatus = true
            };

            _mockDbContext.Setup(db => db.Users).ReturnsDbSet(new List<User>());

            // Act
            var result = _authenticationService.Register(registrationDto);

            // Assert

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
            Assert.Equal("testuser", result.Username);
            Assert.True(result.Expiration > DateTime.UtcNow);

        }

        [Fact]
        public void Register_ShouldThrowException_WhenPasswordIsWeak()
        {
            // Arrange
            var registrationDto = new RegistrationDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "weak", // Weak password
                IdVerificationStatus = true
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _authenticationService.Register(registrationDto));

            Assert.Contains("Password must be at least 10 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.", ex.Message);
        }

        [Fact]
        public void RegisterShouldThrowException_WhenUsernameOrEmailAlreadyExists()
        {
            // Arrange
            var registrationDto = new RegistrationDTO
            {
                Username = "existinguser",
                Email = "exists@example.com",
                Password = "ValidPassword123!",
                IdVerificationStatus = true
            };

            // Setup mock to simulate existing user
            var mockUserSet = new List<User>
            {
                new User
                {
                    Username = "existinguser",
                    Email = "exists@example.com"
                }
            // Replace this line:
            // }.AsQueryable().BuildMockdbSet();

            // With the following lines:
            }.AsQueryable();

            var mockUserDbSet = new Mock<DbSet<User>>();
            mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(mockUserSet.Provider);
            mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(mockUserSet.Expression);
            mockUserDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(mockUserSet.ElementType);
            mockUserDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(mockUserSet.GetEnumerator());

            _mockDbContext.Setup(db => db.Users).Returns(mockUserDbSet.Object);

            var localAuthenticationService = new AuthenticationService(
                _mockDbContext.Object,
                _mockEncryptionService.Object,
                _mockJwtService.Object
            );

            var ex = Assert.Throws<InvalidOperationException>(() => localAuthenticationService.Register(registrationDto));

            Assert.Contains("already exists.", ex.Message);

        }

        [Fact]
        public void Register_ShouldThrowException_WhenIdVerificationIsFalse()
        {
            // Arrange
            var registrationDto = new RegistrationDTO
            {
                Username = "testuser",
                Email = "new@exampl.com",
                Password = "ValidPassword123!",
                IdVerificationStatus = false // Id verification is false
            };

            // Act & Assert

            var ex = Assert.Throws<InvalidOperationException>(() => _authenticationService.Register(registrationDto));

            Assert.Contains("ID verification must be completed before registration.", ex.Message);



        }
    }
}