using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using WebAppVIP.vipUserQuerySvcRef;

namespace WebAppVIP.Symantec
{
    public class vipUserQuerySvcClient
    {
        static QueryServicePortClient vipUserQueryClient;

        public vipUserQuerySvcClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            EndpointAddress address = new EndpointAddress("https://vipuserservices-auth.verisign.com/vipuserservices/QueryService_1_1");

            vipUserQueryClient = new QueryServicePortClient(binding, address);
            vipUserQueryClient.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "DotNetSample");
        }

        //helper function to get a random transaction_id
        private static string getRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }

        public ArrayList getOOBInfo(String userId)
        {
            GetUserInfoRequestType userReqT = new GetUserInfoRequestType();
            GetUserInfoResponseType userRespT;
            ArrayList userOOBInfo = new ArrayList();

            String smsName = "not present";
            String smsNumber = "not present";
            String voiceName = "not present";
            String voiceNumber = "not present";

            userReqT.requestId = getRandomString();
            userReqT.userId = userId;

            try
            {
                userRespT = vipUserQueryClient.getUserInfo(userReqT);
            }
            catch (Exception)
            {
                userOOBInfo.Add("serviceError:Service Error!");
                return userOOBInfo;

            }

            //check for OOB credentials and replace default strings if found
            for (int i = 0; i < Convert.ToInt32(userRespT.numBindings); i++)
            {
                String credType = userRespT.credentialBindingDetail[i].credentialType.ToString();
                
                if (credType.Equals("SMS_OTP"))
                {
                    smsName = userRespT.credentialBindingDetail[i].bindingDetail.friendlyName;
                    smsNumber = userRespT.credentialBindingDetail[i].credentialId;
                }

                if (credType.Equals("VOICE_OTP"))
                {
                    voiceName = userRespT.credentialBindingDetail[i].bindingDetail.friendlyName;
                    voiceNumber = userRespT.credentialBindingDetail[i].credentialId;
                }
            }

            userOOBInfo.Add("SMS^" + smsName + "^" + smsNumber);
            userOOBInfo.Add("Voice^" + voiceName + "^" + voiceNumber);

            return userOOBInfo;
        }
        
        
        public ArrayList getUserInfo(String userId)
        {
            GetUserInfoRequestType userReqT = new GetUserInfoRequestType();
            GetUserInfoResponseType userRespT;
            ArrayList userInfo = new ArrayList();


            userReqT.requestId = getRandomString();
            userReqT.userId = userId;

            try
            {
                userRespT = vipUserQueryClient.getUserInfo(userReqT);
            }
            catch (Exception)
            {
                userInfo.Add("serviceError:Service Error!");        
                return userInfo;

            }
            //add the transaction information
            userInfo.Add("requestTime^" + DateTime.Now.ToString());
            userInfo.Add("requestId^" + userRespT.requestId);
            userInfo.Add("requestStatus^" + userRespT.statusMessage);
            

            //add the user information
            userInfo.Add("userName^" + userRespT.userId);
            userInfo.Add("userStatus^" + userRespT.userStatus);
            userInfo.Add("userPin^" + Convert.ToString(userRespT.isPinSet));

            //add VIP Credential information
            userInfo.Add("credentialCount^" + userRespT.numBindings);

            for (int i = 0; i < Convert.ToInt32(userRespT.numBindings); i++)
            {
                userInfo.Add("credentialId" + (i + 1) + "^" + userRespT.credentialBindingDetail[i].credentialId);
                userInfo.Add("credentialName" + (i + 1) + "^" + userRespT.credentialBindingDetail[i].bindingDetail.friendlyName);
                userInfo.Add("credentialType" + (i + 1) + "^" + userRespT.credentialBindingDetail[i].credentialType);
                userInfo.Add("credentialStatus" + (i + 1) + "^" + userRespT.credentialBindingDetail[i].credentialStatus);
                userInfo.Add("credentialUsed" + (i + 1) + "^" + userRespT.credentialBindingDetail[i].bindingDetail.lastAuthnTime);
            }
            return userInfo;
        }
    }
}