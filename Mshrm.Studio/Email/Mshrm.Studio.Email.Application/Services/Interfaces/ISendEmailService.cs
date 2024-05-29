namespace Mshrm.Studio.Email.Application.Services.Interfaces
{
    public interface ISendEmailService
    {
        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="email">The email to send</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="content">The body</param>
        /// <param name="link">The button link</param>
        /// <returns>If the email job was sent</returns>
        public bool SendEmail(string email, string subject, string content, string link);
    }
}
