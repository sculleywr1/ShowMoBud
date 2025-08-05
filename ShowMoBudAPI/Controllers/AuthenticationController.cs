using ShowMoBudAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ShowMoBudAPI.DTO.DTOModels;

namespace ShowMoBudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private IAuthenticationService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService @object, ILogger<AuthenticationController> logger)
        {
            this._logger = logger;
            this._authService = @object;
        }

        [HttpPost("register")]
        public IActionResult Register(RegistrationDTO registrationDTO)
        {
            try
            {

                var jwtResponse = _authService.Register(registrationDTO);
                _logger.LogInformation($"User {registrationDTO.Username} registered successfully.");
                return Ok(jwtResponse);

            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Registration failed for user {registrationDTO.Username}: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Operation error during registration for user {registrationDTO.Username}: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred during registration for user {registrationDTO.Username}: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred during Registration.");
            }
        }
    }
}
