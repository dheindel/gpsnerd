<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MaterPage1.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <form method="post" action="<%= Url.Action("Download") %>">
        <div id="fake-data" class="align-center">
            <div class="input-area width75 align-center">
                <h4>Truck Info</h4>
                <p><em>Click on the map or enter an address to set the origin and destination.</em></p>
                <div class="input-row">
                    <label for="startAddress">Origin:</label>
                    <input type="text" id="startAddress" name="startAddress"/>
                    <div id="mapStart"></div>
                    <input type="hidden" id="startLat" name="startLat" />
                    <input type="hidden" id="startLng" name="startLng" />
                </div>
                <div class="input-row">
                    <label for="endAddress">Destination:</label>
                    <input type="text" id="endAddress" name="endAddress" />
                    <div id="mapEnd"></div>
                    <input type="hidden" id="endLat" name="endLat" />
                    <input type="hidden" id="endLng" name="endLng" />
                </div>
                <div class="input-cmds">
                    <input id="submit" type="submit" value="Download Data" />
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        var mapStart;
        var mapEnd;
        // Main entry point...runs on document ready
        $(document).ready(function () {
            // Create and load the initial map
            mapStart = $("#mapStart").gmap3(
            {
                lat: 43.0566,
                lng: -89.4511,
                zoom: 2
            });
            mapStart.setTypeRoadMap();
            mapStart.onclickReverseGeocode(function (data) {
                $("#startAddress").val(data);
            });
            mapStart.onclickGetLatLng(function (latlng) {
                setOrigin(latlng[0], latlng[1]);
            });
            mapEnd = $("#mapEnd").gmap3(
            {
                lat: 43.0566,
                lng: -89.4511,
                zoom: 2
            });
            mapEnd.setTypeRoadMap();
            mapEnd.onclickReverseGeocode(function (data) {
                $("#endAddress").val(data);
            });
            mapEnd.onclickGetLatLng(function (latlng) {
                setDestination(latlng[0], latlng[1]);
            });
            $("#startAddress").blur(function () {
                var addr = $(this).val();
                $.fn.gmap3.geoCodeAddress(addr, function (latlng) {
                    setOrigin(latlng.lat(), latlng.lng());
                });
            });
            $("#endAddress").blur(function () {
                var addr = $(this).val();
                $.fn.gmap3.geoCodeAddress(addr, function (latlng) {
                    setDestination(latlng.lat(), latlng.lng());
                });
            });

            // hook up the client side validation
            $("#submit").click(function () {
                return isValide();
            });
        });

        function setOrigin(lat, lng) {
            $("#startLat").val(lat);
            $("#startLng").val(lng);
            mapStart.clear();
            mapStart.addMarkerByLatLng(lat, lng, "origin");
        }

        function setDestination(lat, lng) {
            $("#endLat").val(lat);
            $("#endLng").val(lng);
            mapEnd.clear();
            mapEnd.addMarkerByLatLng(lat, lng, "destination");
        }

        function isValide() {
            var msg = "";
            if ($("#startLat").val() === "" || $("#startLng").val() === "") {
                msg += "<li>An <strong>origin</strong> is required.</li>";
            }
            if ($("#endLat").val() === "" || $("#endLng").val() === "") {
                msg += "<li>A <strong>destination</strong> is required.</li>";
            }
            if (msg != "") {
                var title = "Form Errors...";
                msg = "<p>Fix the following:</p><ul>" + msg + "</ul>";
                growl(title, msg);
                return false;
            }
            return true;
        }

    </script>
</asp:Content>
