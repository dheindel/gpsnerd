<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Mobile1.master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Tracker.Data.Entities.Truck>>" %>
<%@ Import Namespace="Tracker.Data.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="truck-list">
        <div><em>Select one of your trucks.</em></div>
        <ul>
            <% foreach(Truck truck in Model) { %>
            <li>
                <a id="truck-<%= truck.Id %>" class="button blue-btn" href="<%= Url.Action("Trip", "Mobile", new{truckId=truck.Id}) %>">
                    <%= Html.Encode(truck.Name) %><br />
                    (<%= Html.Encode(truck.Type) %>)
                </a>
            </li>
            <% } %>
        </ul>
    </div>
</asp:Content>