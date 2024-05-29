namespace Mshrm.Studio.Email.Api.Models.Pocos.Email.Interfaces
{
    /// <summary>
    /// Interface for an email
    /// </summary>
    public interface IEmail
    {
        /// <summary>
        /// Email subject
        /// </summary>
        /// <returns>An email subject - unless overridden, string.Empty</returns>
        public string Subject { get; }

        /// <summary>
        /// The raw email body
        /// </summary>
        /// <returns>Raw email body</returns>
        public string? RawBody { get; }

        /// <summary>
        /// The email body
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Any values to replace in the email
        /// </summary>
        public List<KeyValuePair<string, string>> ReplaceValues { get; }

        /// <summary>
        /// Generate the body of an email
        /// </summary>
        /// <returns>The body of the email</returns>
        string? GenerateEmailBody();

        /// <summary>
        /// Get the raw email body
        /// </summary>
        /// <returns>Raw email body for body generation</returns>
        string GetRawEmailBody();

        /// <summary>
        /// Get the subject - override otherwise this will be empty
        /// </summary>
        /// <returns>An email subject - unless overriden, string.Empty</returns>
        public string GetSubject();
    }
}
