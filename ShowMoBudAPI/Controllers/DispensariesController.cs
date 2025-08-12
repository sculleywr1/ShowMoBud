using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ShowMoBudAPI.Models;

namespace ShowMoBudAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DispensariesController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    public DispensariesController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpGet("nearby/osm")]
    public async Task<IEnumerable<Dispensary>> GetNearbyOsm(double lat, double lng, double radiusMiles = 10)
    {
        var radiusMeters = radiusMiles * 1609.34;
        var query = $@"[out:json];node[""shop""=""cannabis""](around:{radiusMeters},{lat},{lng});out;";

        var client = _clientFactory.CreateClient();
        using var content = new StringContent(query, Encoding.UTF8, "text/plain");
        using var response = await client.PostAsync("https://overpass-api.de/api/interpreter", content);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var elements = doc.RootElement.GetProperty("elements").EnumerateArray();

        return elements.Select(e => new Dispensary
        {
            Name = e.TryGetProperty("tags", out var tags) && tags.TryGetProperty("name", out var n) ? n.GetString() ?? "Unnamed" : "Unnamed",
            Latitude = e.GetProperty("lat").GetDouble(),
            Longitude = e.GetProperty("lon").GetDouble(),
            Address = e.TryGetProperty("tags", out var t) && t.TryGetProperty("addr:street", out var street) ? street.GetString() : null
        }).ToList();
    }
}

