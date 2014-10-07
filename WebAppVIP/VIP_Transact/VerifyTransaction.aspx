<%@ Page Title="Transaction Verification Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerifyTransaction.aspx.cs" Inherits="WebAppVIP.VIP_Transact.VerifyTransaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-wrapper">
        Here's a simple example of how to use VIP APIs to confirm the details of a transaction. Verification can be done via SMS and Voice. When SMS is used, the end-user must type the received code
        in the web application to confirm it. For Voice verification, the confirmation is done on the phone, and the web application verifies the user response automatically. To use this feature, you must 
        be logged in with a <b>Username</b> that matches a <b>VIP Username</b> in the VIP Service. If you have either an SMS or Voice VIP Credentials registered, they can be used for transaction verification.
        If not, a phone number can be supplied. If the <b>Username</b> does not exist locally, simply use the Register link to create it and use this link to verify that the <b>VIP Username</b> exists in the 
        VIP service.<a target="_blank" href="https://manager.vip.symantec.com/">Login to VIP Manager...</a>
    </div>
    <br />
    <hr />
    <asp:LoginView ID="TransactionView" runat="server" ViewStateMode="Disabled">
        <AnonymousTemplate>
            <p>
                <h5>Please <a id="loginLink" runat="server" href="~/Account/Login?ReturnUrl=/VIP_Transact/VerifyTransaction.aspx">Log In</a> or <a id="registerLink" runat="server" href="~/Account/Register?ReturnUrl=/VIP_User/VIPUserDetails.aspx">Register</a> to retrieve your VIP Credentials!</h5>
            </p>
        </AnonymousTemplate>
        <LoggedInTemplate>
            <h5>Funds transfer authorization using VIP out-of-band verification</h5>
            <asp:Table ID="TransactOOBCredentials" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        SMS Phone Number:
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="3">
                        <asp:Label ID="SMSTransNumberLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        Voice Phone Number:
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="3">
                        <asp:Label ID="VoiceTransNumberLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        Voice/SMS <b>On-Page</b> Authorization
                        <br />
                        <asp:RadioButton ID="SMSInPageRadioButton" runat="server" AutoPostBack="True" GroupName="OOBTransact" />
                        <b>SMS</b>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                        <asp:RadioButton ID="VoiceInPageRadioButton" runat="server" AutoPostBack="True" GroupName="OOBTransact" />
                        <b>Voice</b
                    </asp:TableCell>
                    <asp:TableCell>
                         <hr width="1" size="40">
                    </asp:TableCell>
                    <asp:TableCell>
                        Voice <b>On-Phone</b>Authorization
                        <br />
                        <asp:RadioButton ID="VoiceInPhoneRadioButton" runat="server" AutoPostBack="True" GroupName="OOBTransact" />
                        <b>Voice</b>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        From Account
                    </asp:TableCell>
                    <asp:TableCell>
                        To Account
                    </asp:TableCell>
                    <asp:TableCell>
                        &nbsp
                        &nbsp
                        Amount (e.g 130.25)
                    </asp:TableCell>
                    <asp:TableCell>
                        Action
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:DropDownList ID="FromAccountList" runat="server">
                            <asp:ListItem>Savings - *7846</asp:ListItem>
                            <asp:ListItem>Checking - *5318</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ToAccountList" runat="server">
                            <asp:ListItem>Brokerage - *6429</asp:ListItem>
                            <asp:ListItem>Student Checking - *3569</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell>
                        $&nbsp
                        <asp:TextBox ID="AmountBox" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="</br>Please enter a valid amount!" ControlToValidate="AmountBox" ValidationExpression="^\d+(\.\d\d)?$" Display="Dynamic"></asp:RegularExpressionValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="TransferOOBTransactionButton" runat="server" Text="Transfer" OnClick ="TransferOOBTransactionButton_Click"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="5">
                        <asp:Label ID="OOBTransactionDetailsLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="OOBTransactIdTextLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="OOBTransactIdLabel" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                       
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="CancelOOBTransactionButton" runat="server" Text="Cancel Transaction" OnClick ="CancelOOBTransactionButton_Click"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </LoggedInTemplate>
    </asp:LoginView>
    <hr />

    <h3>For SMS/Voice On-Page Authorization, enter Verification Code here:</h3>
    <br />
    <asp:TextBox ID="OOBVerifCodeBox" runat="server" CssClass="username" Width="140px"></asp:TextBox>
    <asp:Button ID="OOBVerifCodeButton" runat="server" Text="Authorize" />
    <asp:CheckBox ID="RefreshCheck" runat="server" Checked="True" />
    <b>Refresh VIP Credential information</b>
    <br />
    <h5>
        <asp:Label ID="ResponseLabel" runat="server" CssClass="message-info"></asp:Label>
    </h5>
    <br />
    <hr />
    <br />
    <br />
</asp:Content>
