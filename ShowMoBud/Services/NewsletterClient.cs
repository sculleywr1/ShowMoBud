// ShowMoBud/Services/NewsletterClient.cs
using ShowMoBud.DTO;
using ShowMoBud.Models;
using ShowMoBud.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace ShowMoBud.Services;

public class NewsletterClient : INewsletterClient
{
    private readonly HttpClient _http;
    private const string Base = "api/newsletter";

    public NewsletterClient(HttpClient http) => _http = http;

    public async Task<NewsletterSignupDto> SubscribeAsync(NewsletterSignupDto signup, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync($"{Base}/signup", signup, ct);
        if (response.IsSuccessStatusCode)
        {
            var saved = await response.Content.ReadFromJsonAsync<NewsletterSignupDto>(cancellationToken: ct);
            return saved!;
        }

        if (response.StatusCode == HttpStatusCode.Conflict)
            throw new InvalidOperationException("Email already subscribed.");

        var body = await response.Content.ReadAsStringAsync(ct);
        throw new HttpRequestException($"Signup failed ({(int)response.StatusCode}). {body}");
    }

    public Task<List<NewsletterSignupDto>> GetAllAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<List<NewsletterSignupDto>>($"{Base}", ct)!;
}
