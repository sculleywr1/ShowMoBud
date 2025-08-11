using ShowMoBud.DTO;

namespace ShowMoBud.Services.Interfaces
{
    public interface INewsletterClient
    {

        Task<NewsletterSignupDto> SubscribeAsync(NewsletterSignupDto signup, CancellationToken ct = default);

    }
}
