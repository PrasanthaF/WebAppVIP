<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeafultProvider.aspx.cs" Inherits="WebAppVIP.DeafultProvider" %>
<%@ Import Namespace="System.Web.Profile" %>
<%@ Import Namespace="System.Configuration.Provider" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

  <h3>Default Profile Provider Information Page</h3>

  <table border="1">
    <tr>
      <td>Provider Name</td>
      <td><asp:Label id="ProviderNameLabel" runat="server" /></td>
    </tr>
    <tr>
      <td>Provider Type</td>
      <td><asp:Label id="ProviderTypeLabel" runat="server" /></td>
    </tr>
    <tr>
      <td>Provider Description</td>
      <td><asp:Label id="ProviderDescriptionLabel" runat="server" /></td>
    </tr>
  </table>

    <h3>Enabled Profiles and Types</h3>
<%
foreach (ProviderBase p in ProfileManager.Providers)
  Response.Write("Name: " + p.Name + ", Type: " + p.GetType() + "<br />");
%>
    <asp:Login ID="Login1" runat="server" PasswordLabelText="Password + OTP:">
        <LayoutTemplate>
            <table cellpadding="1" cellspacing="0" style="border-collapse:collapse;">
                <tr>
                    <td>
                        <table cellpadding="0">
                            <tr>
                                <td align="center" colspan="2">Log In</td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="OtpLabel" runat="server" AssociatedControlID="OneTimePassword">OTP:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="OneTimePassword" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="OTPRequired" runat="server" ControlToValidate="OneTimePassword" ErrorMessage="OTP is required." ToolTip="OTP is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color:Red;">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:Login>

</asp:Content>

