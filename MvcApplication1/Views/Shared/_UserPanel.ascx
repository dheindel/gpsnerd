<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="user-panel-wrap" class="blue">
    <span class="left"><%= Html.Encode( Session["FullName"] ) %></span>
    <div class="right">
        <ul>
            <li><%= Html.ActionLink("Log Off", "LogOff", "Account") %></li>
            <li><%= Html.ActionLink("Add Truck", "AddTruck", "Truck") %></li>
            <li><%= Html.ActionLink("Home", "Index", "Home") %></li>
            <li><%= Html.ActionLink("Mobile", "Index", "Mobile") %></li>
            <li><%= Html.ActionLink("Manage Truck", "Manage", "Truck", null, new{id="manage-truck"}) %></li>
        </ul>
    </div>
</div>