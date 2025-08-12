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

            // Fetch nearby dispensaries from the API
            var hits = await Dispensaries.GetNearbyAsync(_userLat, _userLng, _radiusMiles);

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
        // Record type for deserializing geolocation results from JS interop
        private record GeoPosition(GeoCoords coords);
        private record GeoCoords(double latitude, double longitude);
    }
}
