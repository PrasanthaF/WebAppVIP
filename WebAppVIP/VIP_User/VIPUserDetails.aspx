<%@ Page Title="VIP Details Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VIPUserDetails.aspx.cs" Inherits="WebAppVIP.VIP_User.VIPUserDetails" %>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-wrapper">
        Here's a simple example of how to display the user details returned by the VIP service and validate a Security Code from either one of the associated VIP Credentials.
        To use this feature, you must be logged in with a <b>Username</b> that matches a <b>VIP Username</b> in the VIP Service. If the <b>Username</b>
        does not exist locally, simply use the Register link to create it and use this link to verify that the <b>VIP Username</b> exists in the VIP service.
        <a target="_blank" href="https://manager.vip.symantec.com/">Login to VIP Manager...</a>
    </div>
    <br />
    <hr />
    <h3>User Information returned by VIP Services</h3>
        <asp:LoginView ID="UserLoginView" runat="server" ViewStateMode="Disabled">
            <AnonymousTemplate>
                <p>
                    <h5>Please <a id="loginLink" runat="server" href="~/Account/Login?ReturnUrl=/VIP_User/VIPUserDetails.aspx">Log In</a> or <a id="registerLink" runat="server" href="~/Account/Register?ReturnUrl=/VIP_User/VIPUserDetails.aspx">Register</a> to display your VIP Credentials!</h5>
                </p>
            </AnonymousTemplate>
            <LoggedInTemplate>
                <h5>VIP Request Transaction</h5>
                <table style="width: 100%;">
                    <tr>
                        <td>Transaction ID&nbsp;</td>
                        <td>Transaction Time&nbsp;</td>
                        <td>Transaction Status&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="transactionId" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="transactionTime" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="transactionStatus" runat="server"></asp:Label>&nbsp;</td>
                    </tr>
                </table>
                <h5>VIP User Details</h5>
                <table style="width: 100%;">
                    <tr>
                        <td>User ID&nbsp;</td>
                        <td>User Status&nbsp;</td>
                        <td>User PIN Set&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="userId" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="userStatus" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="userPin" runat="server"></asp:Label>&nbsp;</td>
                    </tr>
                </table>
                <h5>VIP Credentials</h5>
                <table style="width: 100%;">
                    <tr>
                        <td>Credential ID&nbsp;</td>
                        <td>Credential Name&nbsp;</td>
                        <td>Credential Type&nbsp;</td>
                        <td>Credential Status&nbsp;</td>
                        <td>Credential Last Used&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="CredentialId1" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialName1" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialType1" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialStatus1" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialUsed1" runat="server"></asp:Label>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="CredentialId2" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialName2" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialType2" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialStatus2" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialUsed2" runat="server"></asp:Label>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="CredentialId3" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialName3" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialType3" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialStatus3" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialUsed3" runat="server"></asp:Label>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="CredentialId4" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialName4" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialType4" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialStatus4" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialUsed4" runat="server"></asp:Label>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="CredentialId5" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialName5" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialType5" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialStatus5" runat="server"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="CredentialUsed5" runat="server"></asp:Label>&nbsp;</td>
                    </tr>
                </table>
            </LoggedInTemplate>
        </asp:LoginView>
    <hr />
    <p>

        <h3>Enter a Security Code from any of the above VIP Credentials to Validate</h3>
        * <em>STANDARD_OTP</em> credentials only, with an <em>ENABLED</em> status
        <br />
        <br />
        <asp:TextBox ID="securityCodeBox" runat="server" CssClass="username" Width="140px"></asp:TextBox>
        <asp:Button ID="validateButton" runat="server" Text="Validate" />
        <asp:CheckBox ID="refreshCheck" runat="server" Checked="True" />
        <b>Refresh VIP Credential information</b>
        <h5>
            <asp:Label ID="validateResponseLabel" runat="server" CssClass="message-info"></asp:Label>
        </h5>
    </p>
    <br />
    <hr />
    <br />
</asp:Content>
