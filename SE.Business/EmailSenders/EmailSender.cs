﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
namespace SE.Business.EmailSenders
{
    public class EmailSender : IEmailSender
    {

        private readonly EmailSettings _emailSettings;


        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        [Obsolete]
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
                    
                    // The third parameter is useSSL (true if the client should make an SSL-wrapped
                    // connection to the server; otherwise, false).
                    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, true);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }

   

}
