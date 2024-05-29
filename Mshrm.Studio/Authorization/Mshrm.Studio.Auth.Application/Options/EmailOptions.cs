using System.Security;

namespace Mshrm.Studio.Auth.Api.Models.Options
{
    /// <summary>
    /// Options for email sending
    /// </summary>
    public class EmailOptions
    {
        /// <summary>
        /// If we send emails or not
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The SMTP server address
        /// </summary>
        public required string Server { get; set; }

        /// <summary>
        /// The SMTP server port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Email to send from
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Display name for email send from
        /// </summary>
        public required string EmailDisplayName { get; set; }

        /// <summary>
        /// Password for SMTP server
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Reply to email address
        /// </summary>
        public required string ReplyToEmailAddress { get; set; }
    }
}
