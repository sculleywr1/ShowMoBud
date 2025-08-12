using ShowMoBudAPI.Models;

namespace ShowMoBudAPI.Services.Interfaces
{
    public interface IJwtService
    {

        JwtResponse GenerateToken(string username, Role role);

    }
}
