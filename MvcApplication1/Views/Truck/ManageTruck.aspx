<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MaterPage1.master" Inherits="System.Web.Mvc.ViewPage<Tracker.Data.Entities.Truck>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <form method="post" action="<%= Url.Action("UpdateTruckInfo") %>">
        <div id="update-truck">
            <div class="input-area width50">
                <h4>Truck Info</h4>
                <div class="input-row">
                    <input type="hidden" name="truckId" value="<%= Model.Id %>" />
                    <label for="Name">Vehicle Name:</label>
                    <input id="name" type="text" name="Name" value="<%= Html.Encode(Model.Name) %>" />
                </div>
                <div class="input-row">
                    <label for="Type">Vehicle Type:</label>
                    <input id="type" type="text" name="Type" value="<%= Html.Encode(Model.Type) %>" />
                </div>
                <div class="input-row">
                    <label for="PlateNumber">Vehical Plate #:</label>
                    <input id="plateNumber" type="text" name="PlateNumber" value="<%= Html.Encode(Model.PlateNumber) %>" />
                </div>
                <div class="input-row">
                    <label for="isPrivate">Is Private:</label>
                    <% if(Model.IsPrivate) {%>
                    <input name="IsPrivate" type="radio" value="True" checked="checked" /> Yes
                    <input name="IsPrivate" type="radio" value="False" /> No
                    <% } else { %>
                    <input name="IsPrivate" type="radio" value="True" /> Yes
                    <input name="IsPrivate" type="radio" checked="checked" value="False" /> No
                    <% } %>
                </div>
                <div class="input-cmds">
                    <input id="submit" type="submit" value="Update Truck" />
                </div>
            </div>
        </div>
    </form>

    <form method="post" enctype="multipart/form-data" action="<%= Url.Action("ProcessGpsFile") %>">
        <div id="upload-gps-file">
            <div class="input-area width50">
                <h4>Upload GPS File</h4>
                <div class="align-center">
                    <input type="hidden" name="truckId" value="<%= Model.Id %>" />
                    <input id="file" type="file" name="File" value="<%= Html.Encode(Model.PlateNumber) %>" />
                </div>
                <div class="align-center">
                    <%= Html.ActionLink("Generate Fake Data", "Index", "FakeData") %>
                </div>
                <div class="input-cmds">
                    <input id="submit1" type="submit" value="Upload & Process" />
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        var homeUrl = '<%= Url.Action("Truck", new{truckId=Model.Id}) %>';
        $("#show-list").attr("href", homeUrl);
        $("#submit").click(function () {
            return validate();
        });
        function validate() {
            var msg = "";
            if ($("#name").val() == "") {
                msg += "<li><strong>Truck Name</strong> cannot be blank</li>";
            }
            if ($("#type").val() == "") {
                msg += "<li><strong>Truck Type</strong> cannot be blank</li>";
            }
            if ($("#plateNumber").val() == "") {
                msg += "<li><strong>Truck Plate</strong> cannot be blank</li>";
            }
            if (msg != "") {
                var title = "Form Errors...";
                msg = "<p>Fix the following:</p><ul>" + msg + "</ul>";
                growl(title, msg);
                return false;
            }
            return true;
        }
        $("#submit2").click(function () {
            var msg = "";
            if ($("#file").val() == "") {
                msg += "<li>Select a <strong>file</strong> first</li>";
            }
            if (msg != "") {
                var title = "Form Errors...";
                msg = "<p>Fix the following:</p><ul>" + msg + "</ul>";
                growl(title, msg);
                return false;
            }
            return true;
        });
    </script>
</asp:Content>
