using CMS.ApplicationCore;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mime;
using MimeKit;
using Google.Apis.Gmail.v1.Data;
using CMS.ApplicationCore.DTO;

namespace CMS.Infrastructure
{
    public class GoogleEmailSender:IEmailSender
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string[] _scopes = { GmailService.Scope.GmailSend };
        private readonly string _credentialPath = "token.json";
        private readonly string _applicationName = "Gmail API .NET Quickstart";


        private readonly EmailConfig _config;

        public GoogleEmailSender(EmailConfig emailConfig, string clientId, string clientSecret)
        {
            _config = emailConfig;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }
        public void Send()
        {
            var service = GetService();
            var htmlView = AlternateView.CreateAlternateViewFromString(_config.Body, Encoding.UTF8, MediaTypeNames.Text.Html);
            htmlView.ContentType.CharSet = Encoding.UTF8.WebName;

            using (var smtp = new SmtpClient())
            {
                try {
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(_config.From);
                    foreach (string recipient in _config.To)
                        mailMessage.To.Add(recipient);
                    mailMessage.Subject = _config.Subject; ;
                    mailMessage.Body = _config.Body;
                    mailMessage.AlternateViews.Add(htmlView);

                    var mimeMessage = MimeMessage.CreateFromMailMessage(mailMessage);
                    var gmailMessage = new Message
                    {
                        Raw = Endcode(mimeMessage.ToString())
                    };
                    var request = service.Users.Messages.Send(gmailMessage, "me");
                    request.Execute();

                }
                catch
                {
                    throw new Exception();
                }
            }
            
		}

        private GmailService GetService()
        {
            var service = new GmailService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = getCredential(),
                ApplicationName = _applicationName
            });
            return service;
        }

        private UserCredential getCredential()
        {
            ClientSecrets secret = GetSecretData();
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(secret, _scopes, "user", CancellationToken.None, GetFileDataStore()).Result;
            return credential;
        }
        private FileDataStore GetFileDataStore()
        {
            return new FileDataStore(_credentialPath, true);
        }

        private ClientSecrets GetSecretData()
        {
            ClientSecrets secret = new();
            secret.ClientId = _clientId;
            secret.ClientSecret = _clientSecret;

            return secret;
        }

        private string Endcode(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}
