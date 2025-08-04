using System.ComponentModel.DataAnnotations;

namespace ShowMoBudAPI.DTO
{
    public class PasswordComplexity : ValidationAttribute
    {

        public override bool IsValid(object? value)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
            {
                return false; // Password cannot be null or empty
            }

            // Check for password complexity requirements
            return password.Length >= 10 && 
                   password.Any(char.IsUpper) && 
                   password.Any(char.IsLower) && 
                   password.Any(char.IsDigit) && 
                   password.Any(ch => "!@#$%^&*()_+[]{}|;:,.<>?".Contains(ch));
        }

        public override string FormatErrorMessage(string name)
        {
            // Customize the error message to include the name of the field
            return $"{name} must be at least 10 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";

        }

    }
}
