<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TTSHLogin.aspx.cs" Inherits="FormsAuthAd.TTSHLogin" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>
    <section id="loginForm">
        <h2>Use an active directory account to log in.</h2>

        <p class="validation-summary-errors">
            <asp:Literal runat="server" ID="FailureText" />
        </p>
        <fieldset>
            <legend>Log in Form</legend>
            <ol>
                <li>
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtDomain">Domain Name</asp:Label>
                    <asp:TextBox runat="server" ID="txtDomain" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDomain" CssClass="field-validation-error" ErrorMessage="The user name field is required." />
                </li>
                <li>
                    <asp:Label runat="server" AssociatedControlID="txtUserName">User name</asp:Label>
                    <asp:TextBox runat="server" ID="txtUserName" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUserName" CssClass="field-validation-error" ErrorMessage="The user name field is required." />
                </li>
                <li>
                    <asp:Label runat="server" AssociatedControlID="txtPassword">Password</asp:Label>
                    <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="field-validation-error" ErrorMessage="The password field is required." />
                </li>
                <li>
                    <asp:CheckBox runat="server" ID="RememberMe" />
                    <asp:Label runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Remember me?</asp:Label>
                    <asp:CheckBox ID="chkPersist" runat="server" />
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Persist Cookie</asp:Label>
                </li>

            </ol>
            <asp:Button runat="server" CommandName="Login" Text="Log in" ID="btnLogin" OnClick="btnLogin_Click" />
            <asp:Image ID="imgUser" runat="server"  />
        </fieldset>
    </section>

</asp:Content>
