using FitnessApp.Data;
using FitnessApp.Models;
using Infobip.Api.Client;
using Infobip.Api.Client.Api;
using Infobip.Api.Client.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessApp.Utility
{
    public class SendMemberInvoiceMessage
    {
        private readonly ApplicationDbContext _context;

        public SendMemberInvoiceMessage(ApplicationDbContext context)
        {
            _context = context;
        }
        private static readonly string BASE_URL = "https://lzkrer.api.infobip.com";
        private static readonly string API_KEY = "fa66f2e740c11cf729bec1838f6a7929-c8d5e304-f0b6-4860-9d27-3f4694285a0a";
        private static readonly string SENDER = "InfoSMS";
        private static readonly string RECIPIENT = "962789292164";
        private static string MESSAGE_TEXT = "";

        public  void InvoiceMessageWithoutRemainingAmmount(string id)
            {
                /* Find the Invoice by the Id */
                var invoice = _context.Invoices
                    .Include(q => q.Member)
                    .ThenInclude(q => q.Person)
                    .Where(q => q.Id == id)
                    .FirstOrDefault();
                /* Find Member by Invoice and sent it to member */
                var member = _context.Members
                    .Include(q=>q.Person)
                    .Include(q=>q.GymBundle)
                    .Where(q => q.Id == invoice.MemberId)
                    .FirstOrDefault();
            /* Find the user */
            var user = _context.Users.Where(q => q.Id == member.PersonId).FirstOrDefault();

            MESSAGE_TEXT = $"Hello Dear Member ${user.Email.Substring(0, user.Email.IndexOf("@"))} " +
                      $"you have paid the Invoice bill number : #{invoice.SerialNumber} " +
                      $"With value : {invoice.Userpays.ToString("C")} " +
                      $"At {invoice.UserPayDate.ToString("D")}";


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

        public void InvoiceMessageWitRemainingAmmount(string id)
        {

            /* Find the Invoice by the Id */
            var invoice = _context.Invoices
                .Include(q => q.Member)
                .ThenInclude(q => q.Person)
                .Where(q => q.Id == id)
                .FirstOrDefault();
            /* Find Member by Invoice and sent it to member */
            var member = _context.Members
                .Include(q => q.Person)
                .Include(q => q.GymBundle)
                .Where(q => q.Id == invoice.MemberId)
                .FirstOrDefault();
            /* Find the user */
            var user = _context.Users.Where(q => q.Id == member.PersonId).FirstOrDefault();

            MESSAGE_TEXT = $"Hello Dear Member ${user.Email.Substring(0, user.Email.IndexOf("@"))} " +
                $"you have paid the Invoice bill number : #{invoice.SerialNumber} " +
                $"with value : ${invoice.Userpays.ToString("C")} " +
                $"your Remaining Amount is : ${invoice.RemainingValue.ToString("C")} " +
                $"Please you have to pay before ${member.MembershipTo} ";



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

        public void SendInvoiceMessage(string id)
        {
            var invoice = _context.Invoices
            .Include(q => q.Member)
            .ThenInclude(q => q.Person)
            .Where(q => q.Id == id)
            .FirstOrDefault();

            if (invoice.IsFullyPaid)
            {
                InvoiceMessageWithoutRemainingAmmount(id);
            }
            else
            {
                InvoiceMessageWitRemainingAmmount(id);
            }
        }
    }
}
