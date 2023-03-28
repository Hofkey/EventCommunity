using EventCommunity.Core.Interfaces;
using System.Net;
using System.Net.Mail;

namespace EventCommunity.Infrastructure.Mail
{
    public class MailService : IMailService
    {
        private readonly MailAddress fromAddress;
        private readonly SmtpClient client;

        public MailService(string fromAdress, string fromName, string fromPassword)
        {
            fromAddress = new MailAddress(fromAdress, fromName);

            client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
        }

        public void Send(string content, string subj, string toAdress, string? toName = null)
        {
            var toAddress = new MailAddress(toAdress, toName ?? string.Empty);

            string subject = subj;
            string body = content;

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            client.Send(message);
        }

    }
