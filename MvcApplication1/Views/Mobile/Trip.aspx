<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Mobile1.master" Inherits="System.Web.Mvc.ViewPage<Tracker.Data.Entities.Truck>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <input id="truckId" name="truckId" value="<%= Model.Id %>" type="hidden" />
 	<div id="ctrls">
        <div><em><%=Html.Encode(Model.Name) %></em> (<%= Html.Encode(Model.Type)%>)</div>
        <ul>
            <li><a id="trucks" href="<%= Url.Action("Index", "Truck") %>" class="button blue-btn">Change Truck</a></li>
            <li><a id="toggle" href="#" class="button green-btn">Start Trip</a></li>
            <li><a id="reset" href="#" class="button blue-btn">Clear Trip</a></li>
            <li><a id="map" href="#" class="button blue-btn">Map It</a></li>
        </ul>
 		
 		<div id="count">0 Points</div>
 		<div id="location">Unknown</div>
 		<div id="distance">0 miles</div>
 	</div>
	<div id="mapContainer">
		<!-- This is where Google map will go. --->
	</div>

 
	<script type="text/javascript">
	    $.postify = function (value) {
	        var result = {};

	        var buildResult = function (object, prefix) {
	            for (var key in object) {

	                var postKey = isFinite(key)
                ? (prefix != "" ? prefix : "") + "[" + key + "]"
                : (prefix != "" ? prefix + "." : "") + key;

	                switch (typeof (object[key])) {
	                    case "number": case "string": case "boolean":
	                        result[postKey] = object[key];
	                        break;

	                    case "object":
	                        if (object[key].toUTCString)
	                            result[postKey] = object[key].toUTCString().replace("UTC", "GMT");
	                        else {
	                            buildResult(object[key], postKey != "" ? postKey : key);
	                        }
	                }
	            }
	        };

	        buildResult(value, "");

	        return result;
	    };

	    var ptsSent = 0;
	    var pts = []; 			    // All the GPS points
	    var distIndex = 1; 		    // Index for distance calculation
	    var totalDistance = 0.0;    // Total distance travelled
	    var currentLat = 0.0; 	    // Current latitude
	    var currentLng = 0.0; 	    // Current longitude
	    var accuracy = 0.0; 		// Current accuracy in miles
	    var minDistance = 0.05; 	// Minimum distance (miles) between collected points.
	    var isStarted = false; 	    // Flag tracking the application state.
	    var map = null; 			// The map
	    var markers = []; 		    // Container for the map markers
	    var positionTimer; 		    // The id of the position timer.
	    var postTimer;              // The id of the post timer.
	    var postInterval = 10000;   // Post interval in milliseconds. This allows points to be batched.
	    var tripId;                 // A unique trip id.

        // Setup error handler for ajax
	    $.ajaxSetup({
	        error: function (x, e) {
	            if (x.status == 0) {
	                console.log('You are offline!!\n Please Check Your Network.');
	            } else if (x.status == 404) {
	                console.log('Requested URL not found.');
	            } else if (x.status == 500) {
	                console.log('Internel Server Error.');
	            } else if (e == 'parsererror') {
	                console.log('Error.\nParsing JSON Request failed.');
	            } else if (e == 'timeout') {
	                console.log('Request Time out.');
	            } else {
	                console.log('Unknow Error.\n' + x.responseText);
	            }
	        }
	    });

        // Wire up the event handlers
	    $("#toggle").click(function (evt) {
	        evt.preventDefault();
	        if (!isStarted) {
	            $(this).html("End Trip").removeClass("green-btn").addClass("red-btn");
	            var truckId = $("#truckId").val();
	            var url = '<%= Url.Action("StartTrip", "Mobile") %>';
	            $.get(url, { truckId: truckId }, function (data) {
	                if (data != "") {
	                    tripId = data;
	                    startGps();
	                    isStarted = true;
	                } else {
	                    alert("Could not contact the server.");
	                    $("#toggle").html("Start Trip").removeClass("red-btn").addClass("green-btn");
	                }
	            });
	        } else {
	            $(this).html("Start Trip").removeClass("red-btn").addClass("green-btn");
	            stopGps();
	            isStarted = false;
	        }
	    });
	    $("#reset").click(function (evt) {
	        evt.preventDefault();
	        if (confirm("Clear the data?")) {
	            pts = [];
	            ptsSent = 0;
	            distIndex = 1;
	            totalDistance = 0.0;
	            currentLat = 0;
	            currentLng = 0;
	            accuracy = 0;
	            updateStatus();
	            clearMarkers();
	        }
	    })
	    $("#map").click(function (evt) {
	        evt.preventDefault();
	        showPoints();
	    })

	    function updateStatus() {
	        $("#count").html(pts.length + " Collected, " + ptsSent + " Sent");
	        $("#location").html("(" + currentLat.toFixed(4) + "," + currentLng.toFixed(4) + ") <br />&plusmn;" + accuracy.toFixed(4) + "miles");
	        if (pts.length > 0) {
	            for (var i = distIndex; i < pts.length; i++) {
	                totalDistance += distance(
 					pts[i - 1].coords.latitude,
 					pts[i - 1].coords.longitude,
 					pts[i].coords.latitude,
 					pts[i].coords.longitude
 				);
	            }
	            distIndex = pts.length;
	        }
	        $("#distance").html(totalDistance.toFixed(4) + " miles");
	    }

	    var mapContainer = $("#mapContainer");
	    map = new google.maps.Map(
			mapContainer[0],
			{
			    zoom: 1,
			    center: new google.maps.LatLng(
					0,
					0
				),
			    mapTypeId: google.maps.MapTypeId.ROADMAP
			}
		);

	    function startGps() {
	        // Check to see if this browser supports geolocation.
	        if (navigator.geolocation) {
                // Start the interval timer that sends data to the server
	            postTimer = setInterval(sendPts, postInterval);
	            positionTimer = navigator.geolocation.watchPosition(
					function (position) {
					    if (position.coords.accuracy / 609.344 > 0.5) {	// 609.344 meters per mile
					        // First point has low accuracy (cell phone or IP geolocation)
					        // Ignore all low accuracy points.
					        return;
					    }
					    var dist = distance(currentLat, currentLng, position.coords.latitude, position.coords.longitude);
					    if (dist < minDistance) {
					        // Ignore points that are within a certain distance to the last point.
					        return;
					    }

					    pts.push(position);

					    // Track current position
					    accuracy = position.coords.accuracy / 609.344; // 609.344 meters per mile
					    currentLat = position.coords.latitude;
					    currentLng = position.coords.longitude;

					    // Update the status
					    updateStatus();
					},
					function (error) {
					    console.log("Something went wrong: ", error);
					},
					{
					    timeout: (60 * 1000),
					    maximumAge: (1000),
					    enableHighAccuracy: true
					}
				);

	        } else {
	            alert("Your browser does not support geo-location.");
	        }
	    }

	    function stopGps() {
	        navigator.geolocation.clearWatch(positionTimer);
	        positionTimer = null;
	        clearInterval(postTimer);
	        postTimer = null;
	        sendPts();
	    }

	    var indexOfLastSent = 0;
	    function sendPts() {
	        // Batch the points
	        var count = pts.length;
	        var numToSend = count - indexOfLastSent;
	        if (numToSend <= 0) {
	            return;
	        }

	        // Package up the data
	        var data = new Object();
	        data.id = tripId;

	        var ptsToSend = [];
	        for (var i = indexOfLastSent; i < count; i++) {
	            var pt = new Object();
	            pt.lat = pts[i].coords.latitude;
	            pt.lng = pts[i].coords.longitude;
	            pt.ts = new Date(pts[i].timestamp);
	            ptsToSend.push(pt);
	        }
	        indexOfLastSent = count;
	        data.pts = ptsToSend;

	        var numPoints = ptsToSend.length;
	        var url = '<%= Url.Action("AddLocations", "Mobile") %>';
	        $.post(
                    url, $.postify(data),
                    function (data) {
                        if (data == 'Ok') {
                            ptsSent += numPoints;
                            updateStatus();
                        } else {
                            console.log("post returned: " + data);
                        }
                    });
	    }

	    function distance(lat1, lng1, lat2, lng2) {
	        var radius = 3956.0; // miles

	        var deltaLat = ToRadians(lat2 - lat1);
	        var deltaLng = ToRadians(lng2 - lng1);
	        var sinLat = Math.sin(0.5 * deltaLat);
	        var sinLng = Math.sin(0.5 * deltaLng);
	        var cosLat1 = Math.cos(ToRadians(lat1));
	        var cosLat2 = Math.cos(ToRadians(lat2));
	        var h1 = sinLat * sinLat + cosLat1 * cosLat2 * sinLng * sinLng;
	        var h2 = Math.sqrt(h1);
	        var h3 = 2 * Math.asin(Math.min(1, h2));
	        var distance = radius * h3;
	        return distance;
	    }

	    function ToRadians(degree) {
	        return (degree * (Math.PI / 180));
	    }

	    function showPoints() {
	        clearMarkers();
	        if (pts.length == 0) {
	            map.setCenter(new google.maps.LatLng(0, 0));
	            map.setZoom(1);
	            return;
	        }
	        var maxLat = -500.0;
	        var minLat = 500.0;
	        var maxLng = -500.0;
	        var minLng = 500.0;
	        for (var i = 0; i < pts.length; i++) {
	            var lat = pts[i].coords.latitude;
	            var lng = pts[i].coords.longitude;
	            if (lat > maxLat) { maxLat = lat; }
	            if (lat < minLat) { minLat = lat; }
	            if (lng > maxLng) { maxLng = lng; }
	            if (lng < minLng) { minLng = lng; }
	            addMarker(lat, lng);
	        }
	        if (maxLat == minLat) {
	            maxLat += 0.5;
	            minLat -= 0.5;
	        }
	        if (minLng == maxLng) {
	            minLng -= 0.5;
	            maxLng += 0.5;
	        }
	        var sw = new google.maps.LatLng(minLat, minLng);
	        var ne = new google.maps.LatLng(maxLat, maxLng);
	        var bounds = new google.maps.LatLngBounds(sw, ne);
	        map.fitBounds(bounds);
	    }

	    function clearMarkers() {
	        for (var i = 0; i < markers.length; i++) {
	            markers[i].setMap(null);
	            markers[i] = null;
	        }
	        markers = [];
	    }

	    function addMarker(latitude, longitude, label) {
	        var marker = new google.maps.Marker({
	            map: map,
	            position: new google.maps.LatLng(
					latitude,
					longitude
				),
	            title: (label || "")
	        });
	        markers.push(marker);
	    }
 
	</script>
</asp:Content>