using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShowMoBud.Services;

namespace ShowMoBud.Components
{
    // Base class for the DispensaryMap Blazor component, handles map logic and interop
    public class DispensaryMapBase : ComponentBase
    {
        // Unique map DOM element ID for this component instance
        protected string _mapId = $"map_{Guid.NewGuid():N}";
        // Default search radius in miles
        protected double _radiusMiles = 10;
        // Default user location (St. Louis, MO) if geolocation is unavailable
        protected double _userLat = 38.6270, _userLng = -90.1994;

        // Injected JavaScript runtime for JS interop
        [Inject]
        protected IJSRuntime JS { get; set; }
        // Injected service providing dispensary data
        [Inject]
        protected DispensaryService Dispensaries { get; set; }

        // Called after the component has rendered; initializes the map on first render
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Initialize the map with fallback coordinates and default zoom
                await JS.InvokeVoidAsync("smbMap.initMap", _mapId, _userLat, _userLng, 12);
            }
        }

        // Attempts to find the user's location and update the map with nearby dispensaries
        protected async Task FindNearMe()
        {
            // Try to get the user's current position using browser geolocation
            try
            {
                var pos = await JS.InvokeAsync<GeoPosition>("navigator.geolocation.getCurrentPosition");
                _userLat = pos.coords.latitude;
                _userLng = pos.coords.longitude;
            }
            catch
            {
                // If geolocation fails, fallback to default center (St. Louis)
            }

            // Center the map on the user's location
            await JS.InvokeVoidAsync("smbMap.setView", _userLat, _userLng, 12);

            // Get all dispensaries and filter by those within the search radius
            var all = Dispensaries.GetAll();
            var allAddresses = all.SelectMany(d => d.Addresses).ToList();
            var hits = allAddresses.Where(d => HaversineMiles(_userLat, _userLng, d.Latitude, d.Longitude) <= _radiusMiles)
                          .ToList();

            // Clear any existing markers from the map
            await JS.InvokeVoidAsync("smbMap.clearMarkers");

            // Add a marker for the user's location
            await JS.InvokeVoidAsync("smbMap.addMarker", _userLat, _userLng, "<b>You are here</b>");

            // Add a marker for each nearby dispensary
            foreach (var d in hits)
            {
                var popup = $"<b>{d.Name}</b><br/>{d.Address}";
                await JS.InvokeVoidAsync("smbMap.addMarker", d.Latitude, d.Longitude, popup);
            }
        }

        // Calculates the distance in miles between two latitude/longitude points using the Haversine formula
        private static double HaversineMiles(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 3958.7613; // Earth radius in miles
            double dLat = ToRad(lat2 - lat1);
            double dLon = ToRad(lon2 - lon1);
            lat1 = ToRad(lat1); lat2 = ToRad(lat2);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        // Converts degrees to radians
        private static double ToRad(double deg) => deg * Math.PI / 180.0;

        // Record type for deserializing geolocation results from JS interop
        private record GeoPosition(GeoCoords coords);
        private record GeoCoords(double latitude, double longitude);
    }
}
