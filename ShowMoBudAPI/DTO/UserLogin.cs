using System.ComponentModel.DataAnnotations;

namespace ShowMoBudAPI.DTO
{
    // Simple DTO used for login requests
    public class UserLogin
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}