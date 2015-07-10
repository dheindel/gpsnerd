<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MaterPage1.master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Tracker.Data.Entities.Truck>>" Debug="true" %>
<%@ Import Namespace="Tracker.Data.Entities" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Content/scripts/trucktracker.js") %>"></script>
    <div id="trucks" class="panel">
        <div id="truck-list" class="left">
            <h3>
                Truck List 
            </h3>
            <ul>
                <%  int id = ViewData.Keys.Contains("SelectId") ? (int) ViewData["SelectId"] : -1;
                    foreach(Truck truck in Model) 
                    { %>
                <li>
                    <% if(id==-1 || truck.Id!=id){ %>
                    <a id="truck-<%= truck.Id %>" href="javascript:showTruck(<%= truck.Id %>)">
                    <% }else{ %>
                    <a id="truck-<%= truck.Id %>" class="selected" href="javascript:showTruck(<%= truck.Id %>)">
                    <% } %>
                        <span class="truck">
                            <span class="truck-name"><%= truck.Name %></span> (<span class="truck-type"><%= truck.Type %></span>)
                        </span>
                    </a>
                </li>
                <% } %>
            </ul>
        </div>

        <div id="map" class="align-center">
            <div id="map_canvas" class="line"></div>
        </div>
    </div>

</asp:Content>
