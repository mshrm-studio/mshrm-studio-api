using Microsoft.Extensions.Options;
using Mshrm.Studio.Shared.Helpers;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using Mshrm.Studio.Email.Api.Models.Options;
using Mshrm.Studio.Email.Application.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Mshrm.Studio.Email.Application.Services
{
    /// <summary>
    /// Service for sending emails
    /// </summary>
    public class SendEmailService : ISendEmailService
    {
        /// <summary>
        /// Email options
        /// </summary>
        private readonly EmailOptions _emailOptions;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailService"/> class.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="emailOptions"></param>
        public SendEmailService(IWebHostEnvironment env, IOptions<EmailOptions> emailOptions)
        {
            _env = env;
            _emailOptions = emailOptions.Value;
        }

        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="email">The email to send</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="content">The body</param>
        /// <param name="link">The button link</param>
        /// <returns>If the email job was sent</returns>
        public bool SendEmail(string email, string subject, string content, string link)
        {
            var body = StringUtility.LoadAndReplaceString(
                $"EmailTemplates/Notification.html",
                subject,
                content,
                link,
                "Go",
                DateTime.UtcNow.ToString("yyyy")
            );

            return SendEmail(email, subject, body);
        }

        #region Helpers

        /// <summary>
        /// Send emails
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private bool SendEmail(string email, string subject, string body)
        {
            // Basic check
            if (string.IsNullOrEmpty(email))
                return false;

            // Check that we can send emails
            if (_emailOptions.Enabled)
            {
                // Create client
                var smtp = new SmtpClient
                {
                    Host = _emailOptions.CompanyEmailServer,
                    Port = _emailOptions.CompanyEmailPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_emailOptions.CompanyEmail, _emailOptions.CompanyEmailPassword)
                };

                // Build and send message
                using (var message = new MailMessage() { Subject = subject, Body = body, IsBodyHtml = true })
                {
                    // Set from
                    message.From = new MailAddress(_emailOptions.CompanyEmail, _emailOptions.CompanyEmailDisplayName);

                    // Set bcc if the admin adress isn't the recipient
                    if (!string.IsNullOrEmpty(_emailOptions.AdminEmailAddress) && email.ToLower().Trim() != _emailOptions.AdminEmailAddress.ToLower().Trim())
                        message.Bcc.Add(_emailOptions.AdminEmailAddress);

                    // Set to
                    message.To.Add(email);

                    // Set reply to
                    if (!string.IsNullOrEmpty(_emailOptions.AdminEmailAddress))
                    {
                        message.ReplyToList.Add(new MailAddress(_emailOptions.AdminEmailAddress, _emailOptions.AdminEmailAddress));
                    }

                    // Send message
                    smtp.Send(message);

                    // If we got here then its successful
                    return true;
                }
            }

            // Return true if the emails are disabled
            return true;
        }

        #endregion
    }
}
