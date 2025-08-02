using Microsoft.IdentityModel.Tokens;
using ShowMoBudAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShowMoBudAPI.Services
{
    public class JwtService
    {
        private IConfigurationRoot config;

        public JwtService(IConfigurationRoot config)
        {
            this.config = config;
        }

        public JwtResponse GenerateToken(string username, Role role)
        {
            var secretKey = config["JwtSettings:SecretKey"];
            var issuer = config["JwtSettings:Issuer"];
            var audience = config["JwtSettings:Audience"];
            var expirationMinutes = int.Parse(config["JwtSettings:ExpirationMinutes"] ?? "60");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role.RoleName)
            };

            var expires = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = username,
                Expiration = expires
            };
        }
    }
}
