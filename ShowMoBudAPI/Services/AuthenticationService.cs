using Microsoft.EntityFrameworkCore;
using ShowMoBudAPI.Contexts;
using ShowMoBudAPI.DTO;
using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Models;
using ShowMoBudAPI.Services.Interfaces;

namespace ShowMoBudAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private ShowMoBudContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IJwtService _jwtService;

        public AuthenticationService(ShowMoBudContext context, IEncryptionService encryptionService, IJwtService jwtService)
        {
            _context = context;
            _encryptionService = encryptionService;
            _jwtService = jwtService;
        }

        public JwtResponse Register(RegistrationDTO registrationDto)
        {

            //Confirm that the user has completed ID verification
            if (!registrationDto.IdVerificationStatus)
                throw new InvalidOperationException("ID verification must be completed before registration.");

            //check password complexity
            var passwordComplexity = new PasswordComplexity();
            if (!passwordComplexity.IsValid(registrationDto.Password))
                throw new ArgumentException(passwordComplexity.FormatErrorMessage("Password"));

            //check if username or email already exists
            bool userExists = _context.Users.Any(u => u.Username.ToLower() == registrationDto.Username.ToLower() || u.Email.ToLower() == registrationDto.Email.ToLower());

            if (userExists)
            {
                throw new InvalidOperationException("A user with that username or email already exists.");
            }


            //hash and salt the password
            var salt = _encryptionService.GenerateSalt();
            var hashedPassword = _encryptionService.HashPassword(registrationDto.Password, salt);

            //create new user entity

            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                Username = registrationDto.Username,
                Email = registrationDto.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                IdVerificationStatus = registrationDto.IdVerificationStatus,
                IsActive = true,
                JoinDate = DateTime.UtcNow
            };

            //try to add the new user to the database

            try
            {

                _context.Users.Add(newUser);
                _context.SaveChanges();
                //generate JWT token for the new user

                Role role = new Role { RoleId = 1, RoleName = "Free" }; //default role

                var token = _jwtService.GenerateToken(newUser.Username, role ); //default role is "Free"

                return token;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while registering the user.", ex);

            }

            throw new InvalidOperationException("An unknown error occurred during registration.");
        }
    }
}
