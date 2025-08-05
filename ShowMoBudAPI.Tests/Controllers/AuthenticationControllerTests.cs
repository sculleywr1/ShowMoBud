using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ShowMoBudAPI.Controllers;
using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Models;
using ShowMoBudAPI.Services.Interfaces;

namespace ShowMoBudAPI.Tests.Controllers
{
    public class AuthenticationControllerTests
    {

        private readonly Mock<IAuthenticationService> _mockAuth;
        private readonly AuthenticationController _controller;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationControllerTests()
        {
            this._logger = Mock.Of<ILogger<AuthenticationController>>();
            _mockAuth = new Mock<IAuthenticationService>();
            _controller = new AuthenticationController(_mockAuth.Object, _logger);
        }

        [Fact]
        public void Register_ReturnsOkwithToken()
        {
            // Arrange
            var registrationDTO = new RegistrationDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "testpassword",
                IdVerificationStatus = true
            };

            var expectedToken = new JwtResponse
            {
                Token = "mocked_token",
                Username = registrationDTO.Username,
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            _mockAuth
                .Setup(auth => auth.Register(It.IsAny<RegistrationDTO>()))
                .Returns(expectedToken);

            // Act
            var result = _controller.Register(registrationDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var jwt = Assert.IsType<JwtResponse>(okResult.Value);

            Assert.Equal(expectedToken.Token, jwt.Token);
            Assert.Equal(expectedToken.Username, jwt.Username);

        }

        [Fact]
        public void Register_ReturnsBadRequest_OnException()
        {

            // Arrange
            var registrationDTO = new RegistrationDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Weak123",
                IdVerificationStatus = true
            };

            _mockAuth
                .Setup(auth => auth.Register(It.IsAny<RegistrationDTO>()))
                .Throws(new ArgumentException("Password must be at least 10 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character."));

            // Act
            var result = _controller.Register(registrationDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Password must be at least 10 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.", badRequestResult.Value);
        }
    }
}
