using Forum.Core.Exceptions;
using Forum.Core.Interfaces.Services;
using Forum.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace Forum.Application.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendContactFormData(ContactFormModel contactFormBody, string secretPass)
        {
            if (contactFormBody == null
                || contactFormBody.Email == null
                || contactFormBody.UserId == 0
                || contactFormBody.Subject == null
                || contactFormBody.Message == null) throw new BadRequestException("Required fields missing.");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("TechCavern Forum", "techcavern.forum@gmail.com"));
            email.To.Add(new MailboxAddress("TechCavern Forum", "techcavern.forum@gmail.com"));

            email.Subject = $"[Contact form - from user with id {contactFormBody.UserId}] " + contactFormBody.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"<b>Attached email : {contactFormBody.Email}<br><br> Message: {contactFormBody.Message}<b>" };

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, false);
                await smtpClient.AuthenticateAsync("techcavern.forum@gmail.com", secretPass); 
                await smtpClient.SendAsync(email);
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
