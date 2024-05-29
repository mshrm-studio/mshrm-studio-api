using Microsoft.Extensions.Localization;
using Mshrm.Studio.Email.Api.Models.Entities.Email.Bases;
using Mshrm.Studio.Email.Api.Models.Pocos.Email.Interfaces;
using Mshrm.Studio.Email.Application.Resources;

namespace Mshrm.Studio.Email.Api.Models.Pocos.Email
{
    /// <summary>
    /// The password reset email
    /// </summary>
    public class PasswordResetEmail : EmailBase
    {
        private readonly IStringLocalizer<EmailResource> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordResetEmail"/> class.
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="replaceValues"></param>
        public PasswordResetEmail(IStringLocalizer<EmailResource> localizer, List<KeyValuePair<string, string>> replaceValues) : 
            base(replaceValues)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Generate the body of an email
        /// </summary>
        /// <returns>The body of the email</returns>
        public override string GetRawEmailBody()
        {
            return _localizer["Click the link below to reset your password."];
        }

        /// <summary>
        /// Get the subject - override otherwise this will be empty
        /// </summary>
        /// <returns>An email subject - unless overridden, string.Empty</returns>
        public override string GetSubject()
        {
            return _localizer["Password Reset"];
        }
    }
}
