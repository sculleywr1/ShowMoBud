using ShowMoBudAPI.DTO.DTOModels;
using ShowMoBudAPI.Models;

namespace ShowMoBudAPI.Services.Interfaces
{
    public interface IAuthenticationService
    {

        public JwtResponse Register(RegistrationDTO registrationDto);

    }
}
