<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MaterPage1.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        // Global map variable
        var mapstart;

        // Main entry point...runs on document ready
        $(document).ready(function () {
            // Create and load the initial map
            mapstart = $("#map-area").gmap3(
            {
                lat: 43.0566,
                lng: -89.4511,
                zoom: 2
            });
            $("#address").blur(function () {
                var addr = $(this).val();
                $.fn.gmap3.geoCodeAddress(addr, function (latlng) {
                    updateLatLng(latlng.lat(), latlng.lng());
                    mapstart.setCenter(latlng.lat(), latlng.lng());
                    mapstart.setZoom(14);
                });
            });
            mapstart.setTypeRoadMap();
            mapstart.onclickReverseGeocode(function (address) {
                $("#address").val(address);
            });
            mapstart.onclickGetLatLng(function (latlng) {
                updateLatLng(latlng[0], latlng[1]);
            });
        });
        function updateLatLng(lat, lng) {
            $("#latValue").val(lat);
            $("#lngValue").val(lng);
            mapstart.clear();
            mapstart.addMarkerByLatLng(lat, lng, "Start");
        }
    </script>
    <% using (Html.BeginForm("AddTruck", "Truck")) %>
    <% { %>
        <div id="add-truck">
            <div class="input-area width50">
                <h4>Truck Info</h4>
                <div class="input-row">
                    <label for="truckName">Vehicle Name:</label>
                    <input id="truckName" type="text" name="truckName" />
                </div>
                <div class="input-row">
                    <label for="truckType">Vehicle Type:</label>
                    <input id="truckType" type="text" name="truckType" />
                </div>
                <div class="input-row">
                    <label for="truckPlate">Vehical Plate #:</label>
                    <input id="truckPlate" type="text" name="truckPlate" />
                </div>
                <div class="input-row">
                    <label for="isPrivate">Is Private:</label>
                    <input name="isPrivate" type="radio" value="True" /> Yes
                    <input name="isPrivate" type="radio" checked="checked" value="False" /> No
                </div>
                <div class="input-row">
                    <label for="address">Start Address:</label>
                    <input id="address" name="address" type="text" />
                    <input id="latValue" name="latValue" type="hidden" />
                    <input id="lngValue" name="lngValue" type="hidden" />
                </div>
                <div id="map-area"></div>
                <div class="input-cmds">
                    <input id="submit" type="submit" value="Create Truck" />
                </div>
            </div>
        </div>
    <% } %>

    <script type="text/javascript">
        $("#submit").click(function () {
            return validate();

        });
        function validate() {
            var msg = "";
            if ($("#truckName").val() == "") {
                msg += "<li><strong>Truck Name</strong> cannot be blank</li>";
            }
            if ($("#truckType").val() == "") {
                msg += "<li><strong>Truck Type</strong> cannot be blank</li>";
            }
            if ($("#truckPlate").val() == "") {
                msg += "<li><strong>Truck Plate</strong> cannot be blank</li>";
            }
            if ($("#address").val() == "") {
                msg += "<li><strong>Starting Address</strong> cannot be blank</li>";
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
