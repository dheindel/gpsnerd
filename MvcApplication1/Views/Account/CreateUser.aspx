<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MaterPage1.master" Inherits="System.Web.Mvc.ViewPage<Tracker.Data.Entities.User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="create-wrap">
        <div class="input-area">
            <h4>User Registration</h4>
            <div>
                <form method="post" action="<%= Url.Action("Create") %>">
                    <div class="input-row">
                        <label for="username">User Name:</label>
                        <input type="text" value="<%= Html.Encode(Model.UserName) %>" disabled="disabled" />
                        <input name="userName" type="hidden" value="<%= Html.Encode(Model.UserName) %>" />
                    </div>
                    <div class="input-row">
                        <label for="firstName">First Name:</label>
                        <input id="firstName" name="firstName" type="text" value="<%= Html.Encode(Model.FirstName) %>" />
                    </div>
                    <div class="input-row">
                        <label for="lastName">Last Name:</label>
                        <input id="lastName" name="lastName" type="text" value="<%= Html.Encode(Model.LastName) %>" />
                    </div>
                    <div class="input-row">
                        <label for="email">Email:</label>
                        <input id="email" name="email" type="text" value="<%= Html.Encode(Model.Email) %>" />
                    </div>
                    <div class="input-cmds">
                        <input id="submit" name="submit" type="submit" value="Create User" />
                    </div>
                </form>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $("#submit").click(function () {
            return validate();

        });
        function validate() {
            var msg = "";
            if ($("#firstName").val() == "") {
                msg += "<li><strong>First Name</strong> cannot be blank</li>";
            }
            if ($("#lastName").val() == "") {
                msg += "<li><strong>Last Name</strong> cannot be blank</li>";
            }
            if ($("#email").val() == "") {
                msg += "<li><strong>Email</strong> cannot be blank</li>";
            } else {
                var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
                if (!emailPattern.test($("#email").val())) {
                    msg += "<li><strong>Email</strong> must be a valid email address</li>";
                }
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
