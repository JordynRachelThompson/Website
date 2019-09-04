using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using PortfolioWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PortfolioWebsite.Services
{
    public class EmailService :IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly Microsoft.Extensions.Options.IOptions<StorageOptions> _config;

        public EmailService(IEmailConfiguration emailConfiguration, Microsoft.Extensions.Options.IOptions<StorageOptions> config)        
        {
            _emailConfiguration = emailConfiguration;
            _config = config;
        }

        public bool SendContactMessage(EmailMessage emailMessage)
        {

            var username = _config.Value.SmtpUsername;
            var pass = _config.Value.SmtpPassword;

            if (emailMessage.ToAddresses == "")
            {
                emailMessage.ToAddresses = username;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailMessage.Name, emailMessage.FromAddresses));
            message.To.Add(new MailboxAddress("", emailMessage.ToAddresses));
            
            message.Subject = "Portfolio Website Msg From: " + emailMessage.Name;
            
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                //SSL
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);

                //Remove OAuth 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(username, pass);
                //emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                try
                {
                    emailClient.Send(message);
                    emailClient.Disconnect(true);
                    return true;
                }
                catch (Exception ex)
                {
                    emailClient.Disconnect(true);
                    return false;
                }
            }
        }
    }
}
