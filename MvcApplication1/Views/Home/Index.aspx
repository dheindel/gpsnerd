﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MaterPage1.master" Inherits="System.Web.Mvc.ViewPage" Debug="true" %>
<%@ Import Namespace="Tracker.Data.Entities" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel">
        <img class="left image" src="<%= Url.Content("~/Content/images/Map.jpg") %>" alt="map" width="350px" />
        <div id="home-what">
            <h2>What is this?</h2>
            <p>This web site is a project intended to be a tutorial to learn a number of technologies.
            The following are some of the technologies leveraged:</p>
            <div class="small-col left">
                <ul class="links">
                    <li><a href="http://www.microsoft.com/net/">.NET Framework</a></li>
                    <li><a href="http://www.mysql.com/">MySQL</a></li>
                    <li><a href="http://nhforge.org/Default.aspx">NHibernate</a></li>
                    <li><a href="http://fluentnhibernate.org/">Fluent NHibernate</a></li>
                    <li><a href="http://sourceforge.net/projects/nhibernate/">LINQ to NHibernate</a></li>
                    <li><a href="http://ninject.org/">Ninject</a></li>
                </ul>
            </div>
            <div class="small-col">
                <ul class="links">
                    <li><a href="http://www.asp.net/mvc">ASP.NET MVC</a></li>
                    <li><a href="http://code.google.com/apis/maps/documentation/javascript/">Google Maps API v.3</a></li>
                    <li><a href="http://jquery.com">jQuery</a></li>
                    <li><a href="http://www.dotnetopenauth.net/">DotNetOpenAuth</a></li>
                    <li><a href="http://arduino.com">Arduino</a></li>
                    <li><a href="http://netduino.com">Netduino</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div class="panel">
        <div id="home-next" class="clear">
            <div class="left width50">
                <h2>What's next?</h2>
                <h6>Tutorials</h6>
                <p>The implementation of this site is documented in the following tutorials. As the project matures there will be more tutorias added.</p>
                <ul class="links">
                    <li>
                        <a href="http://blog.bobcravens.com/2010/06/01/TheRepositoryPatternWithLinqToFluentNHibernateAndMySQL.aspx">The Repository Pattern with Linq to Fluent NHibernate and MySQL</a>
                    </li>
                    <li>
                        <a href="http://blog.bobcravens.com/2010/09/16/TheRepositoryPatternPart2.aspx">The Repository Pattern - Part 2</a>
                    </li>
                    <li>
                        <a href="http://blog.bobcravens.com/2010/07/05/UsingNHibernateInASPNETMVC.aspx">Wiring up NHibernate in ASP.NET MVC using Ninject</a>
                    </li>
                    <li>
                        <a href="http://blog.bobcravens.com/2010/06/06/AGoogleMapsVersion3JQueryPlugin.aspx">A Google Maps Version 3 jQuery Plugin</a>
                    </li>
                    <li>
                        <a href="http://blog.bobcravens.com/2010/07/11/AnASPNETMVCTruckTrackerApplicationUsingGoogleMaps.aspx">An ASP.NET MVC Truck Tracker Application using Google Maps</a>
                    </li>
                    <li>
                        <a href="http://blog.bobcravens.com/2010/08/08/OpenIDAndOAuthUsingDotNetOpenAuthInASPNETMVC.aspx">OpenID and OAuth using DotNetOpenAuth in ASP.NET MVC</a>
                    </li>
                    <li>
                        <a href="http://blog.bobcravens.com/2010/08/15/CreateAGPSDataLoggerUsingTheArduino.aspx">Create a GPS Data Logger using the Arduino</a>
                    </li>
                    <li>
                        <a href="http://blog.bobcravens.com/2010/08/22/MakingTheWebGrowlUsingJQuery.aspx">Making The Web Growl Using jQuery</a>
                    </li>
                    <li>
                        <a href="http://blog.bobcravens.com/2010/08/28/TruckTrackerIsLive.aspx">Truck Tracker Is Live</a>
                    </li>
                    <li>
                        <a href="http://blog.bobcravens.com/2010/09/01/GPSUsingTheNetduino.aspx">GPS using the Netduino</a>
                    </li>
                    <li>
                        <a href="http://blog.bobcravens.com/2010/09/06/GPSDataLoggerUsingTheNetduino.aspx">GPS Data Logger using the Netduino</a>
                    </li>
                </ul>
                <h6>Public Trucks</h6>
                <p>Trucks (and yes you can track cars or anything else for that matter) can be registered as <em>private</em> or <em>public</em>. Here is a link to a public truck:</p>
                <a href="<%= Url.Action("Index", "Truck", new{truckId=7}) %>">Public Truck</a>
                <h6>Sign Up</h6>
                <p>The registration process uses service providers (like Google and Twitter) for authentication. Your password is <em>never</em> provided to this site.</p>
            </div>
            <div class="right width50">
                <div id="col2">
                    <img class="image" src="<%= Url.Content("~/Content/images/AddTruck.jpg") %>" alt="add truck" width="350px" />
                    <h6>Subscribe</h6>
                    <p>To keep up with additional Truck Tracker news, subscribe to either my <a href="http://blog.bobcravens.com" alt="blog">blog</a> and <a href="http://twitter.com/rcravens" alt="twitter">twitter</a> account.</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
