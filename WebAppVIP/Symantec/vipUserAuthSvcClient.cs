using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using WebAppVIP.vipUserAuthSvcRef;

namespace WebAppVIP.Symantec
{
    public class vipUserAuthSvcClient
    {
        static AuthenticationServicePortClient vipUserAuthClient;

        public vipUserAuthSvcClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            EndpointAddress address = new EndpointAddress("https://vipuserservices-auth.verisign.com/vipuserservices/AuthenticationService_1_1");

            vipUserAuthClient = new AuthenticationServicePortClient(binding, address);
            vipUserAuthClient.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "DotNetSample");
        }

        //helper function to get a random transaction_id
        private static string getRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }

        public Boolean validateSecurityCode(String userId, String securityCode)
        {
            AuthenticateUserRequestType userReqT = new AuthenticateUserRequestType();
            AuthenticateUserResponseType userRespT;

            OtpAuthDataType otpReqT = new OtpAuthDataType();
            otpReqT.otp = securityCode;

            userReqT.requestId = getRandomString();
            userReqT.userId = userId;
            userReqT.Item = otpReqT;
            
            try
            {
                userRespT = vipUserAuthClient.authenticateUser(userReqT);
            }
            catch (Exception)
            {
                return false;
            }

            if (BitConverter.ToInt16(userRespT.status,0) == 0)
            {
                return true;
            }

            return false;
        }
    }
}