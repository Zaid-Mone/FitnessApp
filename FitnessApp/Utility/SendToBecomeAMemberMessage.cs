using Infobip.Api.Client;
using Infobip.Api.Client.Api;
using Infobip.Api.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessApp.Utility
{
    public class SendToBecomeAMemberMessage
    {
        private static readonly string BASE_URL = "https://198j8x.api.infobip.com";
        private static readonly string API_KEY = "61af7a6525013e553eee9dbedc906675-1065bab4-9405-4e6b-bdb2-f655638b2347";
        private static readonly string SENDER = "InfoSMS";
        private static readonly string RECIPIENT = "962780388117";
        private static string MESSAGE_TEXT = "";
        public static void BecomeAMemberMessage()
        {
            MESSAGE_TEXT = $"Thank you for your registration for fitness training." +
                $"You will receive a message soon with your username and password. Thank You..";

            var configuration = new Configuration()
            {
                BasePath = BASE_URL,
                ApiKeyPrefix = "App",
                ApiKey = API_KEY
            };

            var sendSmsApi = new SendSmsApi(configuration);

            var smsMessage = new SmsTextualMessage()
            {
                From = SENDER,
                Destinations = new List<SmsDestination>()
                {
                    new SmsDestination(to: RECIPIENT)
                },
                Text = MESSAGE_TEXT
            };

            var smsRequest = new SmsAdvancedTextualRequest()
            {
                Messages = new List<SmsTextualMessage>() { smsMessage }
            };

            try
            {
                var smsResponse = sendSmsApi.SendSmsMessage(smsRequest);

                Console.WriteLine("Response: " + smsResponse.Messages.FirstOrDefault());
            }
            catch (ApiException apiException)
            {
                Console.WriteLine("Error occurred! \n\tMessage: {0}\n\tError content", apiException.ErrorContent);
            }
        }
    }
}

