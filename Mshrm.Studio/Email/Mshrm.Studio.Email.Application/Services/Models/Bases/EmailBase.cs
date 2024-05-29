using Mshrm.Studio.Email.Api.Models.Pocos.Email.Interfaces;

namespace Mshrm.Studio.Email.Api.Models.Entities.Email.Bases
{
    /// <summary>
    /// Base email
    /// </summary>
    public abstract class EmailBase : IEmail
    {
        /// <summary>
        /// Any values to replace in the email
        /// </summary>
        public List<KeyValuePair<string, string>> ReplaceValues { get; private set; }

        /// <summary>
        /// Get the subject - override otherwise this will be empty
        /// </summary>
        /// <returns>An email subject - unless overridden, string.Empty</returns>
        public string Subject => GetSubject();

        /// <summary>
        /// Generate the body of an email
        /// </summary>
        /// <returns>The body of the email</returns>
        public string? RawBody => GetRawEmailBody();

        /// <summary>
        /// Generate the body of an email
        /// </summary>
        /// <returns>The body of the email</returns>
        public string Body => GenerateEmailBody();

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailBase"/> class.
        /// </summary>
        /// <param name="replaceValues"></param>
        public EmailBase(List<KeyValuePair<string, string>> replaceValues) 
        {
            ReplaceValues = replaceValues;
        }

        /// <summary>
        /// Generate the body of an email
        /// </summary>
        /// <returns>The body of the email</returns>
        public virtual string GenerateEmailBody()
        {
            // Get the raw content
            var rawContent = GetRawEmailBody();
            if (string.IsNullOrEmpty(rawContent))
                return string.Empty;

            // Replace values if there is anything to replace
            if ((ReplaceValues?.Any() ?? false))
            {
                foreach (var kvp in ReplaceValues)
                {
                    rawContent = rawContent.Replace(kvp.Key, kvp.Value);
                }
            }

            return rawContent;
        }

        /// <summary>
        /// Get the raw email body - override otherwise this will be empty
        /// </summary>
        /// <returns>Raw email body for body generation - unless overriden, string.Empty</returns>
        public virtual string GetRawEmailBody()
        {
            return string.Empty;
        }

        /// <summary>
        /// Get the subject - override otherwise this will be empty
        /// </summary>
        /// <returns>An email subject - unless overridden, string.Empty</returns>
        public virtual string GetSubject()
        {
            return string.Empty;
        }
    }
}
