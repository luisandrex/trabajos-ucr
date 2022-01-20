using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    public class MailService : IMailService
    {
        private readonly MailSettingsModel mailSettings;

        public MailService(IOptions<MailSettingsModel> _mailSettings)
        {
            mailSettings = _mailSettings.Value;
        }

        public async Task SendEmailAsync(EmailContentModel emailContent)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(emailContent.Destination));
            email.Subject = emailContent.Subject;            
            var Body = new TextPart(TextFormat.Html) { Text = emailContent.Body };
            if (emailContent.AttachmentPath != null)
            {
                var Attachment = new MimePart("file", "csv")
                {
                    Content = new MimeContent(File.OpenRead(emailContent.AttachmentPath)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = Path.GetFileName(emailContent.AttachmentPath)
                };
                var multipart = new Multipart("mixed");
                multipart.Add(Body);
                multipart.Add(Attachment);
                email.Body = multipart;
            }
            else
            {
                email.Body = Body;
            }
            var smtpClient = new SmtpClient();
            smtpClient.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(mailSettings.Mail, mailSettings.Password);
            await smtpClient.SendAsync(email);
            smtpClient.Disconnect(true);

        }
    }
}
