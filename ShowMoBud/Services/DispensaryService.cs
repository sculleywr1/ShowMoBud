using System.Net.Http.Json;
using ShowMoBud.Models;

namespace ShowMoBud.Services;

public class DispensaryService
{
    private readonly HttpClient _http;

    public DispensaryService(HttpClient http)
    {
        _http = http;
    }

    public async Task<IReadOnlyList<Dispensary>> GetNearbyAsync(double lat, double lng, double radiusMiles)
    {
        var url = $"api/dispensaries/nearby/osm?lat={lat}&lng={lng}&radiusMiles={radiusMiles}";
        return await _http.GetFromJsonAsync<List<Dispensary>>(url) ?? new List<Dispensary>();
    }
}

