using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using WebAppVIP.Symantec;

namespace WebAppVIP.VIP_OOB
{
    public partial class SendValidate : System.Web.UI.Page
    {

        static vipUserQuerySvcClient vipQueryClient;
        static vipUserAuthSvcClient vipAuthClient;
        static vipUserMgmtSvcClient vipMgmtClient;

        static String smsNumber;
        static String smsName;

        static String voiceNumber;
        static String voiceName;

        private static Boolean checkSecurityCode(String userId, String securityCode)
        {
            vipAuthClient = new vipUserAuthSvcClient();
            return vipAuthClient.validateSecurityCode(userId, securityCode);
        }

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

        private String sendOtp(String userId, String oobType)
        {
            vipMgmtClient = new vipUserMgmtSvcClient();
            
            if (oobType.Equals("SMS"))
            {
                return vipMgmtClient.sendOtp(userId,smsNumber,oobType);
            }
            else
            {
                if (oobType.Equals("Voice"))
                {
                    return vipMgmtClient.sendOtp(userId,voiceNumber,oobType);
                }
                else
                {
                    return "Unknown OOB methid!";
                }
            }
        }

        protected void ButtonSMS_Click(object sender, System.EventArgs e)
        {
            ((Label)UserOOBView.FindControl("SMSStatusLabel")).Text = sendOtp(Page.User.Identity.Name,"SMS");
        }

        protected void ButtonVoice_Click(object sender, System.EventArgs e)
        {
            ((Label)UserOOBView.FindControl("VoiceStatusLabel")).Text = sendOtp(Page.User.Identity.Name, "Voice");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // check if user is logged in
            if (String.IsNullOrEmpty(Page.User.Identity.Name))
            {
                validateResponseLabelOOB.Text = "User is not logged in!";
            }
            else
            {

                if (refreshCheckOOB.Checked)
                {
                    //query VIP service for SMS and Voice credentials
                    getOOBInfo(Page.User.Identity.Name);
                    
                    //if at least one OOB Credential exists, enable the corresponding Radio button and mark it as checked
                    // if both are present, default to SMS
                    // if none are present, disable both, not checked
                    if (smsName.Equals("SMS Credential not present!"))
                    {
                        // if SMSdoes not exist, disable and uncheck SMS
                        ((RadioButton)UserOOBView.FindControl("RadioButtonSMS")).Checked = false;
                        ((RadioButton)UserOOBView.FindControl("RadioButtonSMS")).Enabled = false;

                        // if SMS does not exist, check to see if Voice exists
                        if (voiceName.Equals("Voice Credential not present!"))
                        {
                            //if SMS does not exist AND Voice does not exist, disable and uncheck Voice as well
                            ((RadioButton)UserOOBView.FindControl("RadioButtonVoice")).Checked = false;
                            ((RadioButton)UserOOBView.FindControl("RadioButtonVoice")).Enabled = false;
                        }
                        else
                        {
                            //if SMS does not exist but Voice exists, enable and check Voice
                            ((RadioButton)UserOOBView.FindControl("RadioButtonVoice")).Checked = true;
                            ((RadioButton)UserOOBView.FindControl("RadioButtonVoice")).Enabled = true;
                        }
                    }
                    else
                    {
                        //if SMS exists, enable and check SMS
                        ((RadioButton)UserOOBView.FindControl("RadioButtonSMS")).Checked = true;
                        ((RadioButton)UserOOBView.FindControl("RadioButtonSMS")).Enabled = true;

                        //if SMS exists, check to see if Voice exists
                        if (voiceName.Equals("Voice Credential not present!"))
                        {
                            //if SMS exists AND Voice does not exist, disable and uncheck Voice
                            ((RadioButton)UserOOBView.FindControl("RadioButtonVoice")).Checked = false;
                            ((RadioButton)UserOOBView.FindControl("RadioButtonVoice")).Enabled = false;
                        }
                        else
                        {
                            //if SMS exists AND Voice exists, enable but uncheck Voice (SMS is the default)
                            ((RadioButton)UserOOBView.FindControl("RadioButtonVoice")).Checked = false;
                            ((RadioButton)UserOOBView.FindControl("RadioButtonVoice")).Enabled = true;
                        }
                    }

                    //clean-up the security code
                    if (!String.IsNullOrEmpty(securityCodeBoxOOB.Text))
                    {
                        securityCodeBoxOOB.Text = "";
                    }

                    //un-check refresh
                    refreshCheckOOB.Checked = false;
                }

                //display the registered phone number and their friendly name
                //SMS
                ((Label)UserOOBView.FindControl("SMSNumberLabel")).Text = smsNumber;
                ((Label)UserOOBView.FindControl("SMSNameLabel")).Text = smsName;

                //Voice
                ((Label)UserOOBView.FindControl("VoiceNumberLabel")).Text = voiceNumber;
                ((Label)UserOOBView.FindControl("VoiceNameLabel")).Text = voiceName;

                if (((RadioButton)UserOOBView.FindControl("RadioButtonSMS")).Checked)
                {
                    ((Button)UserOOBView.FindControl("ButtonSMS")).Enabled = true;
                    ((Button)UserOOBView.FindControl("ButtonVoice")).Enabled = false;
                }

                if (((RadioButton)UserOOBView.FindControl("RadioButtonVoice")).Checked)
                {
                    ((Button)UserOOBView.FindControl("ButtonSMS")).Enabled = false;
                    ((Button)UserOOBView.FindControl("ButtonVoice")).Enabled = true;
                }

                // look for an OTP value to validate
                if (!String.IsNullOrEmpty(securityCodeBoxOOB.Text))
                {
                    if (checkSecurityCode(Page.User.Identity.Name, securityCodeBoxOOB.Text))
                    {
                        validateResponseLabelOOB.Text = "Success! Security Code: " + securityCodeBoxOOB.Text + " is valid!";
                    }
                    else
                    {
                        validateResponseLabelOOB.Text = "Error! Security Code: " + securityCodeBoxOOB.Text + " is invalid!";
                    }
                    //clean-up the security code
                    securityCodeBoxOOB.Text = "";
                }
                else
                {
                    validateResponseLabelOOB.Text = "Security Code not provided!";
                }
            }   
        }
    }
}