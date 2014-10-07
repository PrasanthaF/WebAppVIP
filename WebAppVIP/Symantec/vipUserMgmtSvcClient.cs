using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using WebAppVIP.vipUserMgmtSvcRef;

namespace WebAppVIP.Symantec
{
    public class vipUserMgmtSvcClient
    {
        static ManagementServicePortClient vipUserMgmtClient;

        public vipUserMgmtSvcClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            EndpointAddress address = new EndpointAddress("https://vipuserservices-auth.verisign.com/vipuserservices/ManagementService_1_1");

            vipUserMgmtClient = new ManagementServicePortClient(binding, address);
            vipUserMgmtClient.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "DotNetSample");
        }

        //helper function to get a random transaction_id
        private static string getRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }

        public String sendOtp(String userId, String phoneNumber, String oobType)
        {
            SendOtpRequestType userReqT = new SendOtpRequestType();
            SendOtpResponseType userRespT;

            
            userReqT.requestId = getRandomString();
            userReqT.userId = userId;
            
            if (oobType.Equals("SMS")) {
                SmsDeliveryInfoType smsPhone = new SmsDeliveryInfoType();
                smsPhone.phoneNumber = phoneNumber;
                userReqT.Item = smsPhone;
            } else {
                if (oobType.Equals("Voice")) {
                    VoiceDeliveryInfoType voicePhone = new VoiceDeliveryInfoType();
                    voicePhone.phoneNumber = phoneNumber;
                    userReqT.Item = voicePhone;
                } else {
                    return "Unknown delivery type!";
                }
            }

            //ready to send
             try
            {
                userRespT = vipUserMgmtClient.sendOtp(userReqT);
            }
            catch (Exception)
            {
                return "Service Error";
            }

            return "Success!";
        }
    }
}