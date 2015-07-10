﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="openid-wrap" class="panel">
    <form id="openid-form" method="post" action="<%= Url.Action("OpenId", "Account") %>">
        <p>Registered users can <strong>view and add private trucks</strong>. Otherwise feel free to browse
        the <strong>public</strong> trucks in the list below.</p>
        <p>Sign in using one of the providers below.</p>
        <div id="openid-providers" class="blue">
            <ul>
                <li id="openid"></li>
                <li id="myopenid"></li>
                <li id="twitter"></li>
                <li id="google"></li>
                <li id="yahoo"></li>
            </ul>
        </div>
        <div id="other">
            <h3>Your OpendID Provider</h3>
            <input name="openIdUrl" id="openIdUrl" type="text" />
            <a id="signin">Sign In »</a>
        </div>
        <input name="returnUrl" id="returnUrl" type="hidden" />
    </form>
</div>

<script type="text/javascript">
    var providers = {
        myopenid: { action: '<%= Url.Action("OpenId", "Account") %>', url: 'http://myopenid.com' },
        twitter: { action: '<%= Url.Action("OAuth", "Account") %>' },
        facebook: { action: '<%= Url.Action("OpenId", "Account") %>', url: 'http://facebook.com' },
        google: { action: '<%= Url.Action("OpenId", "Account") %>', url: 'http://www.google.com/accounts/o8/id' },
        yahoo: { action: '<%= Url.Action("OpenId", "Account") %>', url: 'http://yahoo.com' }
    };
    $("#openid").click(function () {
        toggleOther();
    });
    $("#signin").click(function () {
        $("#openid-form").submit();
    });

    $("li").not("#openid").click(function () {
        $("#other").hide(500);
        isHidden = true;
        var id = $(this).attr("id");
        var provider = providers[id];
        if (provider.action != undefined) {
            $("#openid-form").attr("action", provider.action);
        }
        if (provider.url != undefined) {
            $("#openIdUrl").val(provider.url);
        }
        $("#openid-form").submit();
    });

    var isHidden = true;
    function toggleOther() {
        if (isHidden) {
            $("#other").slideDown(500);
        } else {
            $("#other").slideUp(500);
        }
        isHidden = !isHidden;
    }
    
</script>