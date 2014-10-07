using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using WebAppVIP.Symantec;

namespace WebAppVIP.VIP_Transact
{
    public partial class VerifyTransaction : System.Web.UI.Page
    {
        static vipUserQuerySvcClient vipQueryClient;
        static vipCredentialSvcClient vipCredentialClient;

        static String smsNumber;
        static String smsName;

        static String voiceNumber;
        static String voiceName;

        static String oobIdMessage = "Transaction Identifier:";

        static bool oobTransactionInitiated;

        static String oobTransactionId;

        static String showMessage;

        static String smsSendMessage;

        private static void getOOBInfo(String userId)
        {
            //query the service for OOB credentials
            vipQueryClient = new vipUserQuerySvcClient();
            ArrayList userOOBList = vipQueryClient.getOOBInfo(userId);

            foreach (Object obj in userOOBList)
            {

                string[] tokenString = obj.ToString().Split(new char[] { '^' });

                switch (tokenString[0])
                {
                    case "SMS":
                        if (tokenString[1].Equals("not present"))
                        {
                            smsName = "SMS Credential not present!";
                            smsNumber = "Phone number not present!";
                        }
                        else
                        {
                            smsName = tokenString[1];
                            smsNumber = tokenString[2];
                        }
                        break;
                    case "Voice":
                        if (tokenString[1].Equals("not present"))
                        {
                            voiceName = "Voice Credential not present!";
                            voiceNumber = "Phone number not present!";
                        }
                        else
                        {
                            voiceName = tokenString[1];
                            voiceNumber = tokenString[2];
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void initOOBTransactionData()
        {
            oobTransactionInitiated = false;

            oobTransactionId = "";

            showMessage = "You have requested a transfer of funds from account ending in *_fromAccount_ to account ending in *_toAccount_, for the amount of $_amountValue_. " +
                "For security purposes, a message has been sent to the number indicated above with the transaction details and an automatically generated Verification Code. " +
                "You can save the transaction identifier below for your records. Please enter the received Verification Code below to authorize this transaction. ";

            smsSendMessage = "Fund transfer alert from account *_fromAccount_ to account *_toAccount_, for _amountValue_ USD. Your Verification Code is: _OTP_";
        }

        protected void CancelOOBTransactionButton_Click(object sender, System.EventArgs e)
        {
            initOOBTransactionData();

            displayOOBDetails(true);

            oobTransactionInitiated = false;

            ResponseLabel.Text = "There is no active transaction";
        }

        private void displayOOBDetails(bool cleanFlag)
        {
            if (!cleanFlag)
            {
                ((Label)TransactionView.FindControl("OOBTransactionDetailsLabel")).Text = showMessage;
                ((Label)TransactionView.FindControl("OOBTransactIdTextLabel")).Text = oobIdMessage;
                ((Label)TransactionView.FindControl("OOBTransactIdLabel")).Text = oobTransactionId;
                ((Button)TransactionView.FindControl("CancelOOBTransactionButton")).Visible = true;
            }
            else
            {
                ((Label)TransactionView.FindControl("OOBTransactionDetailsLabel")).Text = "";
                ((Label)TransactionView.FindControl("OOBTransactIdTextLabel")).Text = "";
                ((Label)TransactionView.FindControl("OOBTransactIdLabel")).Text = "";
                ((Button)TransactionView.FindControl("CancelOOBTransactionButton")).Visible = false;
            }
        }

        protected void TransferOOBTransactionButton_Click(object sender, System.EventArgs e)
        {

            if (!String.IsNullOrEmpty(((TextBox)TransactionView.FindControl("AmountBox")).Text.ToString()))
            {
                if (oobTransactionInitiated)
                {
                    ResponseLabel.Text = "Transaction " + oobTransactionId + " already in progress, please cancel of authorize!";
                }
                else
                {
                    vipCredentialClient = new vipCredentialSvcClient();
                    string[] fromAcct = ((DropDownList)TransactionView.FindControl("FromAccountList")).SelectedValue.ToString().Split(new char[] { '*' });
                    string[] toAcct = ((DropDownList)TransactionView.FindControl("ToAccountList")).SelectedValue.ToString().Split(new char[] { '*' });

                    showMessage = showMessage.Replace("_fromAccount_", fromAcct[1]);
                    showMessage = showMessage.Replace("_toAccount_", toAcct[1]);
                    showMessage = showMessage.Replace("_amountValue_", ((TextBox)TransactionView.FindControl("AmountBox")).Text);

                    smsSendMessage = smsSendMessage.Replace("_fromAccount_", fromAcct[1]);
                    smsSendMessage = smsSendMessage.Replace("_toAccount_", toAcct[1]);
                    smsSendMessage = smsSendMessage.Replace("_amountValue_", ((TextBox)TransactionView.FindControl("AmountBox")).Text);


                    if (((RadioButton)TransactionView.FindControl("SMSInPageRadioButton")).Checked)
                    {
                        
                        oobTransactionId = vipCredentialClient.sendTxnOTP(smsNumber, "SMS", smsSendMessage);
                        
                    }
                    else if (((RadioButton)TransactionView.FindControl("VoiceInPageRadioButton")).Checked)
                    {

                        oobTransactionId = vipCredentialClient.sendTxnOTP(smsNumber, "Voice", smsSendMessage);
                    }

                    else
                    {

                        oobTransactionId = "on_phone_not_implemented";

                    }

                    ResponseLabel.Text = "Transaction " + oobTransactionId + " in progress, authorization required!";

                    oobTransactionInitiated = true;
                }

                //clean-up the amount
                ((TextBox)TransactionView.FindControl("AmountBox")).Text = "";
                displayOOBDetails(false);
            }
            else
            {
                if (oobTransactionInitiated)
                {
                    ResponseLabel.Text = "Transaction " + oobTransactionId + " already in progress, please cancel of authorize!";
                    displayOOBDetails(false);
                }
                else
                {
                    ResponseLabel.Text = "There is no active transaction";
                    displayOOBDetails(true);
                }
            }
        }

        private void initOOBDisplay()
        {
            //query VIP service for SMS and Voice credentials
            getOOBInfo(Page.User.Identity.Name);

            //if at least one OOB Credential exists, enable the corresponding Radio button and mark it as checked
            // if both are present, default to SMS
            // if none are present, disable all, not checked
            if (smsName.Equals("SMS Credential not present!"))
            {
                // if SMSdoes not exist, disable and uncheck SMS
                ((RadioButton)TransactionView.FindControl("SMSInPageRadioButton")).Checked = false;
                ((RadioButton)TransactionView.FindControl("SMSInPageRadioButton")).Enabled = false;

                // if SMS does not exist, check to see if Voice exists
                if (voiceName.Equals("Voice Credential not present!"))
                {
                    //if SMS does not exist AND Voice does not exist, disable and uncheck Voice as well
                    //in-page
                    ((RadioButton)TransactionView.FindControl("VoiceInPageRadioButton")).Checked = false;
                    ((RadioButton)TransactionView.FindControl("VoiceInPageRadioButton")).Enabled = false;
                    //in-phone
                    ((RadioButton)TransactionView.FindControl("VoiceInPhoneRadioButton")).Checked = false;
                    ((RadioButton)TransactionView.FindControl("VoiceInPhoneRadioButton")).Enabled = false;
                }
                else
                {
                    //if SMS does not exist but Voice exists, enable and check Voice
                    //in-page
                    ((RadioButton)TransactionView.FindControl("VoiceInPageRadioButton")).Checked = true;
                    ((RadioButton)TransactionView.FindControl("VoiceInPageRadioButton")).Enabled = true;
                    //in-phone
                    ((RadioButton)TransactionView.FindControl("VoiceInPhoneRadioButton")).Checked = false;
                    ((RadioButton)TransactionView.FindControl("VoiceInPhoneRadioButton")).Enabled = true;
                }
            }
            else
            {
                //if SMS exists, enable and check SMS
                ((RadioButton)TransactionView.FindControl("SMSInPageRadioButton")).Checked = true;
                ((RadioButton)TransactionView.FindControl("SMSInPageRadioButton")).Enabled = true;

                //if SMS exists, check to see if Voice exists
                if (voiceName.Equals("Voice Credential not present!"))
                {
                    //if SMS exists AND Voice does not exist, disable and uncheck Voice
                    ((RadioButton)TransactionView.FindControl("VoiceInPageRadioButton")).Checked = false;
                    ((RadioButton)TransactionView.FindControl("VoiceInPageRadioButton")).Enabled = false;
                    //in-phone
                    ((RadioButton)TransactionView.FindControl("VoiceInPhoneRadioButton")).Checked = false;
                    ((RadioButton)TransactionView.FindControl("VoiceInPhoneRadioButton")).Enabled = false;
                }
                else
                {
                    //if SMS exists AND Voice exists, enable but uncheck Voice (SMS is the default)
                    //in-page
                    ((RadioButton)TransactionView.FindControl("VoiceInPageRadioButton")).Checked = false;
                    ((RadioButton)TransactionView.FindControl("VoiceInPageRadioButton")).Enabled = true;
                    //in-phone
                    ((RadioButton)TransactionView.FindControl("VoiceInPhoneRadioButton")).Checked = false;
                    ((RadioButton)TransactionView.FindControl("VoiceInPhoneRadioButton")).Enabled = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // check if user is logged in
            if (String.IsNullOrEmpty(Page.User.Identity.Name))
            {

                ResponseLabel.Text = "User is not logged in!";
            }
            else
            {
                if (!IsPostBack || RefreshCheck.Checked)
                {

                    //initialize the ytsnactional data
                    initOOBTransactionData();

                    //initialize the OOB option
                    initOOBDisplay();

                    //clean-up the security code, if any
                    if (!String.IsNullOrEmpty(OOBVerifCodeBox.Text))
                    {
                        OOBVerifCodeBox.Text = "";
                    }

                    //un-check refresh
                    RefreshCheck.Checked = false;
                }

                if (oobTransactionInitiated)
                {
                    displayOOBDetails(false);
                    ResponseLabel.Text = "Transaction in progress, authorization required!";
                }
                else
                {
                    displayOOBDetails(true);
                    ResponseLabel.Text = "There is no active transaction";
                }

                //display the registered SMS phone number
                ((Label)TransactionView.FindControl("SMSTransNumberLabel")).Text = smsNumber;

                //display the registered Voice phone number
                ((Label)TransactionView.FindControl("VoiceTransNumberLabel")).Text = voiceNumber;

                //if either SMS or Voice in-page is checked, enable in-page verification
                if (((RadioButton)TransactionView.FindControl("SMSInPageRadioButton")).Checked || ((RadioButton)TransactionView.FindControl("VoiceInPageRadioButton")).Checked)
                {
                    OOBVerifCodeBox.Enabled = true;
                    OOBVerifCodeButton.Enabled = true;
                }
                else
                {
                    OOBVerifCodeBox.Enabled = false;
                    OOBVerifCodeButton.Enabled = false;
                }

                if (oobTransactionInitiated)
                {
                    if (!String.IsNullOrEmpty(OOBVerifCodeBox.Text))
                    {
                        vipCredentialClient = new vipCredentialSvcClient();

                        if (vipCredentialClient.verifyTxnOTP(oobTransactionId, OOBVerifCodeBox.Text))
                        {
                            ResponseLabel.Text = "Success! Transaction: " + oobTransactionId + " was verified!";
                            //re-initialize everything, just like Cancel transaction except for the message
                            initOOBTransactionData();

                            displayOOBDetails(true);

                            oobTransactionInitiated = false;
                        }
                        else
                        {
                            ResponseLabel.Text = "Error! Transaction: " + oobTransactionId + " could not be verified! Please try again or cancel transaction.";
                        }
                    }
                    else
                    {
                        ResponseLabel.Text = "Error! Transaction: " + oobTransactionId + " could not be verified! Please try again or cancel transaction.";
                    }
                    OOBVerifCodeBox.Text = "";
                }
                else
                {
                    ResponseLabel.Text = "There is no active transaction";
                }

            }
        }
    }
}