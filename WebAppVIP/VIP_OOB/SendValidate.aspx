<%@ Page Title="VIP OOB Send Validate Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SendValidate.aspx.cs" Inherits="WebAppVIP.VIP_OOB.SendValidate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-wrapper">
        Here's a simple example of how to send an Security Code via SMS or Voice and validate it. To use this feature, you must be logged in with a <b>Username</b>
        that matches a <b>VIP Username</b> in the VIP Service and have either an SMS or Voice VIP Credentials registered. If the <b>Username</b>
        does not exist locally, simply use the Register link to create it and use this link to verify that the <b>VIP Username</b> exists in the VIP service.
        <a target="_blank" href="https://manager.vip.symantec.com/">Login to VIP Manager...</a>
    </div>
    <br />
    <hr />
    <h3>User Information returned by VIP Services</h3>
    <asp:LoginView ID="UserOOBView" runat="server" ViewStateMode="Disabled">
        <AnonymousTemplate>
            <p>
                <h5>Please <a id="loginLink" runat="server" href="~/Account/Login?ReturnUrl=/VIP_OOB/SendValidate.aspx">Log In</a> or <a id="registerLink" runat="server" href="~/Account/Register?ReturnUrl=/VIP_User/VIPUserDetails.aspx">Register</a> to retrieve your VIP Credentials!</h5>
            </p>
        </AnonymousTemplate>
        <LoggedInTemplate>
            <h5>VIP OOB Credentials</h5>
            <asp:Table ID="OOBCredentials" runat="server">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        Select Credential Type
                    </asp:TableCell>
                    <asp:TableCell>
                        Credential Name
                    </asp:TableCell>
                    <asp:TableCell>
                        Phone Number
                    </asp:TableCell>
                    <asp:TableCell>
                        Action
                    </asp:TableCell>
                    <asp:TableCell>
                        Status
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:RadioButton ID="RadioButtonSMS" runat="server" GroupName="RadioOOB" Checked="False" AutoPostBack="True" />
                    </asp:TableCell>
                    <asp:TableCell>
                        SMS
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="SMSNameLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="SMSNumberLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="ButtonSMS" runat="server" Text="Text Me!" OnClick="ButtonSMS_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="SMSStatusLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:RadioButton ID="RadioButtonVoice" runat="server" GroupName="RadioOOB" AutoPostBack="True" />
                    </asp:TableCell>
                    <asp:TableCell>
                       Voice
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="VoiceNameLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="VoiceNumberLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell Enabled="True">
                        <asp:Button ID="ButtonVoice" runat="server" Text="Call Me!" OnClick="ButtonVoice_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="VoiceStatusLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </LoggedInTemplate>
    </asp:LoginView>
    <hr />

    <h3>Enter the received Security Code to Validate</h3>
    <br />
    <asp:TextBox ID="securityCodeBoxOOB" runat="server" CssClass="username" Width="140px"></asp:TextBox>
    <asp:Button ID="validateButtonOOB" runat="server" Text="Validate" />
    <asp:CheckBox ID="refreshCheckOOB" runat="server" Checked="True" />
    <b>Refresh VIP Credential information</b>
    <h5>
        <asp:Label ID="validateResponseLabelOOB" runat="server" CssClass="message-info"></asp:Label>
    </h5>
    <br />
    <hr />
    <br />
    <br />
    </asp:Content>
