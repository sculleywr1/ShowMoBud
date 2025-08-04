namespace ShowMoBudAPI.Services.Interfaces
{
    public interface IEncryptionService
    {

        byte[] GenerateSalt();
        byte[] HashPassword(string password, byte[] salt);

    }
}
