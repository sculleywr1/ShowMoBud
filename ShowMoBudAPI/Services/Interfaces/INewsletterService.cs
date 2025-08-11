using ShowMoBudAPI.Models;

namespace ShowMoBudAPI.Services.Interfaces
{
    public interface INewsletterService
    {

        Task<NewsletterSignup> AddAsync(NewsletterSignup newsletterSignup, CancellationToken ct = default);
        Task<List<NewsletterSignup>> GetAllAsync(CancellationToken ct = default);

    }
}
