// Global map variable
var map;

// Main entry point...runs on document ready
$(document).ready(function () {
    // Create and load the initial map
    map = $("#map_canvas").gmap3(
            {
                lat: 43.0566,
                lng: -89.4511,
                zoom: 2
            });
    map.setTypeRoadMap();

    var selectedTruck = $("#truck-list a.selected");
    if (selectedTruck.length!=0) {
            showTruck(null);
    }
});

// AJAX error handling
$.ajaxSetup({
    "error": function (XMLHttpRequest, textStatus, errorThrown) {
        var title = "Error";
        var msg = "<p><strong>Text Status: </strong>" + textStatus + "</p><p><strong>Error: </strong>" + errorThrown + "</p>";
        growl(title, msg);
    }
});

// AJAX fetch the truck info
function getInfo(rootUrl) {
    var url = rootUrl + 'Info';
    $.getJSON(url, function (data) {
        map.setCenter(data.location.lat, data.location.lng);
        map.setZoom(10);
        map.addPath(data.history);
        // we have the lat/lng...geocode to get address
        $.fn.gmap3.geoCodeLatLng(data.location.lat, data.location.lng, function (address) {
            var latlng = data.location.lat + ", " + data.location.lng;
            map.addMarkerByLatLng(data.location.lat, data.location.lng, data.name, createInfo(data.name, data.plate, data.type, latlng, address, data.user));
        });

    });
}

// Helper to create the map info bubble
function createInfo(title, plate, type, latlng, location, user) {
    return '<div class="popup"><h1 class="popup-title">' + title + '</h1><div id="popup-body"><p><span class="popup-label">owner:</span> ' + user + '</p><p><span class="popup-label">plate:</span> ' + plate + '</p><p><span class="popup-label">type:</span> ' + type + '</p><p><span class="popup-label">lat/lng:</span> ' + latlng + '</p><p><span class="popup-label">location:</span> ' + location + '</p></div></div>';
}

// List object click handler
function showTruck(id) {
    parseUrl();
    if (id == null) { id = urlId; }
    var rootUrl = urlRoot + "/" + urlAction + "/" + id + "/";

    map.clear();
    getInfo(rootUrl);

    $(".selected").removeClass("selected");
    $("#truck-" + id).addClass("selected");
    var elem = $("#manage-truck");
    if (elem !== undefined && elem.length !== 0) {
        var href = rootUrl + "Manage";
        $("#manage-truck").attr('href', href).show();
    }
}

var urlRoot = "";
var urlAction = "truck";
var urlMethod = "Index";
var urlId = 0;
function parseUrl() {
    var rootUrl = window.location.href;
    var index = rootUrl.toLowerCase().lastIndexOf("truck");
    urlRoot = rootUrl.substring(0, index - 1);
    var path = rootUrl.substring(index + 5, rootUrl.length);
    var parts = path.split("/");
    urlId = parts[1];
}
