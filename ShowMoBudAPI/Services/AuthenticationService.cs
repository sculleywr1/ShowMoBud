using Microsoft.EntityFrameworkCore;
using ShowMoBudAPI.Contexts;
using ShowMoBudAPI.DTO;
using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Models;
using ShowMoBudAPI.Services.Interfaces;
using System.Security.Cryptography;

namespace ShowMoBudAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private ShowMoBudContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IJwtService _jwtService;

        // Static dummy values used to perform a fake PBKDF2 when a user is not found,
        // ensuring the service still does the same amount of work and preventing
        // timing-based user enumeration attacks.
        private static readonly byte[] s_dummySalt;
        private static byte[]? s_dummyHash;
        private static readonly object s_dummyLock = new();

        public AuthenticationService(ShowMoBudContext context, IEncryptionService encryptionService, IJwtService jwtService)
        {
            _context = context;
            _encryptionService = encryptionService;
            _jwtService = jwtService;

            // Ensure a dummy salt exists (generated once per app load).
            // Use Fill so bytes are cryptographically strong and not predictable.
            // Static constructor cannot use the injected encryption service, so generate salt here.
            if (s_dummySalt == null)
            {
                // This branch will never be hit because s_dummySalt is readonly and assigned in the static ctor below.
            }

            // Lazily initialize the dummy hash exactly once in a thread-safe way.
            // This precomputes a PBKDF2 result for a fixed dummy password so comparisons
            // against it are consistent and of equal cost to real user checks.
            if (s_dummyHash == null)
            {
                lock (s_dummyLock)
                {
                    if (s_dummyHash == null)
                    {
                        try
                        {
                            // Use a fixed dummy password string for the precomputed value.
                            s_dummyHash = _encryptionService.HashPassword("ShowMoBudDummyPassword!@#", s_dummySalt);
                        }
                        catch
                        {
                            // If hashing fails for any unlikely reason, use a zero-filled buffer of the expected length
                            // to preserve comparison behaviour. The encryption service derives 64 bytes by default.
                            s_dummyHash = new byte[64];
                        }
                    }
                }
            }
        }

        // Static constructor to initialize readonly static salt
        static AuthenticationService()
        {
            s_dummySalt = new byte[25];
            RandomNumberGenerator.Fill(s_dummySalt);
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

        // Verifies the provided username and password.
        // Throws InvalidOperationException("Username or Password does not exist") for any failure (no detail exposed).
        // Uses CryptographicOperations.FixedTimeEquals to avoid timing attacks.
        public JwtResponse Login(UserLogin login)
        {
            if (login is null)
                throw new ArgumentNullException(nameof(login));

            var username = login.Username?.Trim();
            if (string.IsNullOrEmpty(username))
                throw new InvalidOperationException("Username or Password does not exist");

            // Find user by username (case-insensitive)
            var user = _context.Users.SingleOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            // Prepare variables for hashing and comparison.
            byte[] computedHash;
            byte[] storedHash;

            try
            {
                if (user == null || user.PasswordSalt == null || user.PasswordHash == null)
                {
                    // User not found or missing credentials:
                    // run a dummy PBKDF2 using the static dummy salt to equalize work.
                    computedHash = _encryptionService.HashPassword(login.Password, s_dummySalt);

                    // Use the precomputed dummy hash as the comparison target.
                    // s_dummyHash is initialized in the constructor to match PBKDF2 length/format.
                    storedHash = s_dummyHash!;
                }
                else
                {
                    // Real user exists: compute PBKDF2 using the user's salt and compare against stored hash.
                    computedHash = _encryptionService.HashPassword(login.Password, user.PasswordSalt);
                    storedHash = user.PasswordHash;
                }
            }
            catch
            {
                // Any error during hashing -> generic failure (do not reveal details)
                throw new InvalidOperationException("Username or Password does not exist");
            }

            // Use fixed-time comparison to prevent timing attacks
            bool verified = CryptographicOperations.FixedTimeEquals(computedHash, storedHash);

            if (!verified || user == null)
                throw new InvalidOperationException("Username or Password does not exist");

            // Authentication successful — generate token.
            // For now, use a default role like registration; adjust to pull real roles as needed.
            Role role = new Role { RoleId = 1, RoleName = "Free" };

            var token = _jwtService.GenerateToken(user.Username, role);
            return token;
        }
    }
}
