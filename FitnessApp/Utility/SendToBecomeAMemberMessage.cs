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
        private static readonly string BASE_URL = "https://lzkrer.api.infobip.com";
        private static readonly string API_KEY = "fa66f2e740c11cf729bec1838f6a7929-c8d5e304-f0b6-4860-9d27-3f4694285a0a";
        private static readonly string SENDER = "InfoSMS";
        private static readonly string RECIPIENT = "962789292164";
        private static  string MESSAGE_TEXT = "";
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

