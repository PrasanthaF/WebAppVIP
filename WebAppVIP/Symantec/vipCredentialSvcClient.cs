using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using WebAppVIP.vipCredentialSvcRef;

namespace WebAppVIP.Symantec
{
    public class vipCredentialSvcClient
    {
        static vipSoapInterfaceClient vipCredentialClient;

        static String baseURL = "https://services-auth.vip.symantec.com";

        public vipCredentialSvcClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            EndpointAddress address = new EndpointAddress(baseURL);

            vipCredentialClient = new vipSoapInterfaceClient(binding, address);
            vipCredentialClient.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "DotNetSample");
        }

        //helper function to get a random transaction_id
        private static string getRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }

        public bool verifyTxnOTP(String txnId, String otpValue)
        {
            EndpointAddress txnAddress = new EndpointAddress(baseURL + "/txn/soap");
            vipCredentialClient.Endpoint.Address = txnAddress;

            VerifyTxnOTPType userReqT = new VerifyTxnOTPType();
            VerifyTxnOTPResponseType userRespT;

            userReqT.Id = getRandomString();
            userReqT.TxnId = txnId;
            userReqT.TxnOTP = otpValue;
            userReqT.Version = "1.0";

            try
            {
                userRespT = vipCredentialClient.VerifyTxnOTP(userReqT);
            }
            catch (Exception)
            {
                return false;
            }

            if (BitConverter.ToInt16(userRespT.Status.ReasonCode, 0) == 0)
            {
                return true;
            }

            return false;

        }

        public String sendTxnOTP(String phoneNumber, String oobType, String transactMessage)
        {
            EndpointAddress txnAddress = new EndpointAddress(baseURL + "/txn/soap");
            vipCredentialClient.Endpoint.Address = txnAddress;

            DeliverTxnOTPType userReqT = new DeliverTxnOTPType();
            DeliverTxnOTPResponseType userRespT;

            DestinationType destT = new DestinationType();
            destT.Value = phoneNumber;

            userReqT.Id = getRandomString();
            userReqT.Version = "2.0";
            userReqT.Destination = destT;
            
            if (oobType.Equals("SMS")) {
                destT.type = TokenType.SMS;

                SMSDeliveryInfoType smsReqT = new SMSDeliveryInfoType();
                smsReqT.Message = transactMessage;

                userReqT.SMSDeliveryInfo = smsReqT;

            } else if (oobType.Equals("Voice")) {
                destT.type = TokenType.Voice;
            } else {
                return oobType + " - Unknown TxnDelivery Type!";
            }

            try
            {
                userRespT = vipCredentialClient.DeliverTxnOTP(userReqT);
            }
            catch (Exception)
            {
                return "serviceError: Service Error!";
            }

            return userRespT.TxnId;
        }
    }
}