namespace ShowMoBudAPI.Models
{
    public class JwtResponse
    {

        public string Token { get; set; } = null!;
        public string Username { get; set; } = null!;
        public DateTime Expiration { get; set; }

    }
}
