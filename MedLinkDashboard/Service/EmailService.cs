using MedLinkDashboard.IService;
using MedLinkDashboard.ViewModels;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MedLinkDashboard.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string resetLink)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email-templates", "ResetPasswordTemplate.html");
            var htmlContent = await File.ReadAllTextAsync(templatePath);

            // Replace placeholder with real link
            htmlContent = htmlContent.Replace("{{ResetLink}}", resetLink);

            var client = new SendGridClient(_settings.ApiKey);
            var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "Reset your password", htmlContent);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }

    }
}
