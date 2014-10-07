<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebAppVIP._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Sample ASP.NET Web Application</h2>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h1>Symantec Validation and Identity Protection Service (VIP)</h1>
    <h3>SOAP Web Services API - ASP.NET (C#) Integration Examples</h3>
    <ol class="round">
        <li class="one">
            <h5><a href="/VIP_User/VIPUserDetails.aspx">Display and Validate User</a></h5>
            Click here for an example of using the getUserInfo(userId) and authenticateUser(userId, securityCode) APIs to retrieve and display the user details returned by the VIP service, and validate any of the associated standard VIP Credentials.
        </li>
        <li class="two">
            <h5><a href="/VIP_OOB/SendValidate.aspx">Using Out-Of-Band Security Codes to Authenticate</a></h5>
            Click here for an example of using the sendSecurityCode(userId) and authenticateUser(userId, securityCode) APIs to distribute a Security Code via SMS or Voice and validate it.
        </li>
        <li class="three">
            <h5><a href="/VIP_Transact/VerifyTransaction.aspx">Using Out-Of-Band to Verify a Transaction</a></h5>
            Click here for an example of using the verifyTransaction(transactionDetails) API to to send transactions data via SMS or Voice, and confir/deny transaction per user input.
        </li>
    </ol>
</asp:Content>
