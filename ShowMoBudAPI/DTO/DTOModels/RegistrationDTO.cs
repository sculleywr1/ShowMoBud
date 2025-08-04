using System.ComponentModel.DataAnnotations;

namespace ShowMoBudAPI.DTO.DTOModels
{
    public class RegistrationDTO
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [PasswordComplexity]
        public string Password { get; set; } = null!;
        [Required]
        public bool IdVerificationStatus { get; set; } = false;

    }
}
