using System.ComponentModel.DataAnnotations;

namespace ShowMoBudAPI.DTO.DTOModels
{
    public class NewsletterSignupDto
    {
        
        public int SignupId { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "First name can't be longer than 100 characters")]
        public string FirstName { get; set; } = "";
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";
        [Phone(ErrorMessage = "Invalid phone number format")]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

    }
}
