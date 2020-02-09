using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Web.Infrastructure.EmailSenders
{
    public class EmailSender : IEmailService
    {

        private readonly EmailSettings _emailSettings;


        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void Send(string mailTo, string subject, string message, bool isHtml = false)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string subject, string message, Encoding encoding, bool isHtml = false)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string mailCc, string mailBcc, string subject, string message, bool isHtml = false)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml = false)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(string mailTo, string subject, string message, bool isHtml = false)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

                mimeMessage.To.Add(new MailboxAddress(mailTo));

                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;


                    // The third parameter is useSSL (true if the client should make an SSL-wrapped
                    // connection to the server; otherwise, false).
                    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, false);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }

        public Task SendAsync(string mailTo, string subject, string message, Encoding encoding, bool isHtml = false)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, bool isHtml = false)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml = false)
        {
            throw new NotImplementedException();
        }
    }

   

}
