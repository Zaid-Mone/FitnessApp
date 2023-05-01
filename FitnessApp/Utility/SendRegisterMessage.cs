using Infobip.Api.Client;
using Infobip.Api.Client.Api;
using Infobip.Api.Client.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessApp.Utility
{

        public class SendRegisterMessage
        {
            private static readonly string BASE_URL = "https://198j8x.api.infobip.com";
            private static readonly string API_KEY = "61af7a6525013e553eee9dbedc906675-1065bab4-9405-4e6b-bdb2-f655638b2347";
            private static readonly string SENDER = "InfoSMS";
            private static readonly string RECIPIENT = "962780388117";
            private static  string MESSAGE_TEXT = "";





        // Email = @email.com
            public static void RegisterMemberMessage(string firstname, string username, string password) 
            {
                // message ="Dear Member {username} Welocme to Fitness Training your username:{Username }
                // and your password :{password}"

                MESSAGE_TEXT = $"Dear Member {firstname} Welocme to Fitness Training your username:" +
                        $"{username } and your password :{password}";
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

            public static void RegisterAdminMessage(string firstname, string username, string password) 
            {
                // message ="Dear Admin {username} Welocme to Fitness Training your username:{Username }
                // and your password :{password}"
                 MESSAGE_TEXT = $"Dear Admin {firstname} Welocme to Fitness Training your username:" +
                    $"{username }and your password :{password}";
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

            public static void RegisterTrainerMessage(string firstname,string username,string password) 
            {
                // message ="Dear Trainer {username} Welocme to Fitness Training your username:{Username }
                // and your password :{password}"
                MESSAGE_TEXT = $"Dear Trainer {firstname} Welocme to Fitness Training your username:" +
                    $"{username }and your password :{password}";
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

