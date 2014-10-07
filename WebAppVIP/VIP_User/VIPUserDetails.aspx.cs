using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using WebAppVIP.Symantec;

namespace WebAppVIP.VIP_User
{


    public partial class VIPUserDetails : System.Web.UI.Page
    {
        static vipUserQuerySvcClient vipQueryClient;
        static vipUserAuthSvcClient vipAuthClient;

        static String requestId;
        static String requestTime;
        static String requestStatus;

        static String userIdentifier;
        static String userStatus;
        static String userPin;

        //static String credentialCount;
        static ArrayList credentialId = new ArrayList();
        static ArrayList credentialName = new ArrayList();
        static ArrayList credentialType = new ArrayList();
        static ArrayList credentialLastUsed = new ArrayList();
        static ArrayList credentialStatus = new ArrayList();

        private static void getUserInfo(String userId)
        {
            vipQueryClient = new vipUserQuerySvcClient();
            ArrayList userInfoList = vipQueryClient.getUserInfo(userId);

            
            //clean-up the existing list of ids
            credentialId.Clear();
            credentialName.Clear();
            credentialType.Clear();
            credentialLastUsed.Clear();
            credentialStatus.Clear();

            foreach (Object obj in userInfoList)
            {

                string[] tokenString = obj.ToString().Split(new char[]{'^'});

                switch (tokenString[0]) {
                    case "requestId":
                        requestId = tokenString[1];
                        break;
                    case "requestTime":
                        requestTime = tokenString[1];
                        break;
                    case "requestStatus":
                        requestStatus = tokenString[1];
                        break;
                    case "userName":
                        userIdentifier = tokenString[1];
                        break;
                    case "userStatus":
                        userStatus = tokenString[1];
                        break;
                    case "userPin":
                        userPin = tokenString[1];
                        break;
                    case "credentialId1":
                        credentialId.Add(tokenString[1]);
                        break;
                    case "credentialId2":
                        credentialId.Add(tokenString[1]);
                        break;
                    case "credentialId3":
                        credentialId.Add(tokenString[1]);
                        break;
                    case "credentialId4":
                        credentialId.Add(tokenString[1]);
                        break;
                    case "credentialId5":
                        credentialId.Add(tokenString[1]);
                        break;
                    case "credentialName1":
                        credentialName.Add(tokenString[1]);
                        break;
                    case "credentialName2":
                        credentialName.Add(tokenString[1]);
                        break;
                    case "credentialName3":
                        credentialName.Add(tokenString[1]);
                        break;
                    case "credentialName4":
                        credentialName.Add(tokenString[1]);
                        break;
                    case "credentialName5":
                        credentialName.Add(tokenString[1]);
                        break;
                    case "credentialType1":
                        credentialType.Add(tokenString[1]);
                        break;
                    case "credentialType2":
                        credentialType.Add(tokenString[1]);
                        break;
                    case "credentialType3":
                        credentialType.Add(tokenString[1]);
                        break;
                    case "credentialType4":
                        credentialType.Add(tokenString[1]);
                        break;
                    case "credentialType5":
                        credentialType.Add(tokenString[1]);
                        break;
                    case "credentialStatus1":
                        credentialStatus.Add(tokenString[1]);
                        break;
                    case "credentialStatus2":
                        credentialStatus.Add(tokenString[1]);
                        break;
                    case "credentialStatus3":
                        credentialStatus.Add(tokenString[1]);
                        break;
                    case "credentialStatus4":
                        credentialStatus.Add(tokenString[1]);
                        break;
                    case "credentialStatus5":
                        credentialStatus.Add(tokenString[1]);
                        break;
                    case "credentialUsed1":
                        credentialLastUsed.Add(tokenString[1]);
                        break;
                    case "credentialUsed2":
                        credentialLastUsed.Add(tokenString[1]);
                        break;
                    case "credentialUsed3":
                        credentialLastUsed.Add(tokenString[1]);
                        break;
                    case "credentialUsed4":
                        credentialLastUsed.Add(tokenString[1]);
                        break;
                    case "credentialUsed5":
                        credentialLastUsed.Add(tokenString[1]);
                        break;
                    default:
                        break;
                }
            }
        }

        private static Boolean checkSecurityCode(String userId, String securityCode)
        {
            vipAuthClient = new vipUserAuthSvcClient();
            return vipAuthClient.validateSecurityCode(userId, securityCode);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Page.User.Identity.Name))
            {
                validateResponseLabel.Text = "User is not logged in!";
            }
            else
            {
                if (!String.IsNullOrEmpty(securityCodeBox.Text))
                {
                    if (checkSecurityCode(Page.User.Identity.Name, securityCodeBox.Text))
                    {
                        validateResponseLabel.Text = "Success! Security Code: " + securityCodeBox.Text + " is valid!";
                    }
                    else
                    {
                        validateResponseLabel.Text = "Error! Security Code: " + securityCodeBox.Text + " is invalid!";
                    }
                    //clean-up the security code
                    securityCodeBox.Text = "";
                }
                else
                {
                    validateResponseLabel.Text = "Security Code not provided!";
                } 

                if (refreshCheck.Checked)
                {
                    getUserInfo(Page.User.Identity.Name);

                    //clean-up the security code
                    if (!String.IsNullOrEmpty(securityCodeBox.Text))
                    {
                        securityCodeBox.Text = "";
                    }
                }
                
                //populate transaction data
                ((Label)UserLoginView.FindControl("transactionId")).Text = requestId;
                ((Label)UserLoginView.FindControl("transactionTime")).Text = requestTime;
                ((Label)UserLoginView.FindControl("transactionStatus")).Text = requestStatus;

                //populate user data
                ((Label)UserLoginView.FindControl("userId")).Text = userIdentifier;
                ((Label)UserLoginView.FindControl("userStatus")).Text = userStatus;
                ((Label)UserLoginView.FindControl("userPin")).Text = userPin;
                
                //populate credential data
                int id = 1;
                foreach (Object obj in credentialId)
                {
                    ((Label)UserLoginView.FindControl("CredentialId" + id)).Text = obj.ToString();
                    id++;
                }

                id = 1;
                foreach (Object obj in credentialName)
                {
                    ((Label)UserLoginView.FindControl("CredentialName" + id)).Text = obj.ToString();
                    id++;
                }

                id = 1;
                foreach (Object obj in credentialType)
                {
                    ((Label)UserLoginView.FindControl("CredentialType" + id)).Text = obj.ToString();
                    id++;
                }

                id = 1;
                foreach (Object obj in credentialStatus)
                {
                    ((Label)UserLoginView.FindControl("CredentialStatus" + id)).Text = obj.ToString();
                    id++;
                }

                id = 1;
                foreach (Object obj in credentialLastUsed)
                {
                    ((Label)UserLoginView.FindControl("CredentialUsed" + id)).Text = obj.ToString();
                    id++;
                }  
            }
        }
    }
}