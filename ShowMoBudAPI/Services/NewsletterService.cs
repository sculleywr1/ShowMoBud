using Microsoft.EntityFrameworkCore;
using ShowMoBudAPI.Contexts;
using ShowMoBudAPI.Models;
using ShowMoBudAPI.Services.Interfaces;

namespace ShowMoBudAPI.Services
{
    public class NewsletterService : INewsletterService
    {
        private readonly ShowMoBudContext _db;

        public NewsletterService(ShowMoBudContext db)
        {
            _db = db;
        }

        public async Task<NewsletterSignup> AddAsync(NewsletterSignup newsletterSignup, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(newsletterSignup.FirstName))
                throw new ArgumentException("First name is required.", nameof(newsletterSignup));
            if (string.IsNullOrWhiteSpace(newsletterSignup.Email))
                throw new ArgumentException("Email is required.", nameof(newsletterSignup));

            _db.NewsletterSignups.Add(newsletterSignup);

            try
            {
                await _db.SaveChangesAsync(ct);
                return newsletterSignup;
            }
            catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
            {

                throw new InvalidOperationException("This email is already subscribed.", ex);

            }
        }

        public Task<List<NewsletterSignup>> GetAllAsync(CancellationToken ct = default) =>
            _db.NewsletterSignups
                .AsNoTracking()
                .OrderBy(s => s.SignupId)
                .ToListAsync(ct);

        private static bool IsUniqueEmailViolation(DbUpdateException ex) =>
            ex.InnerException?.Message.Contains("UX_NewsletterSignups_Email", StringComparison.OrdinalIgnoreCase) == true
           || ex.InnerException?.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) == true;
    }
}
