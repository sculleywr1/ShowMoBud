// Define a global object 'smbMap' for map interop functions used by Blazor
window.smbMap = (function () {

    // Private variables to hold the Leaflet map instance and marker layer group
    let map, markersLayer;

    // Initializes the Leaflet map in the specified DOM element with given coordinates and zoom level
    function initMap(domId, lat, lng, zoom) {
        if (map) { 
            map.remove(); // Remove existing map if re-initializing
            map = null; 
        }
        map = L.map(domId, { zoomControl: true }).setView([lat, lng], zoom || 12);

        // Add OpenStreetMap tile layer
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; OpenStreetMap contributors'
        }).addTo(map);

        // Create a layer group for markers and add it to the map
        markersLayer = L.layerGroup().addTo(map);
    }

    // Sets the map view to the specified latitude, longitude, and optional zoom level
    function setView(lat, lng, zoom) {
        if (!map) return; 
        map.setView([lat, lng], zoom || map.getZoom());
    }

    // Removes all markers from the map
    function clearMarkers() {
        if (!markersLayer) return; 
        markersLayer.clearLayers();
    }

    // Adds a marker to the map at the specified coordinates, with optional popup HTML content
    function addMarker(lat, lng, popupHtml) {
        if (!markersLayer) return;
        const marker = L.marker([lat, lng]);
        if (popupHtml) {
            marker.bindPopup(popupHtml);
        }
        marker.addTo(markersLayer);
    }

    // Expose public API for Blazor to call
    return {
        initMap,
        setView,
        clearMarkers,
        addMarker
    };

})();