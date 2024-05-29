using Microsoft.Extensions.Localization;
using Mshrm.Studio.Email.Api.Models.Entities.Email.Bases;
using Mshrm.Studio.Email.Application.Resources;

namespace Mshrm.Studio.Email.Api.Models.Pocos.Email
{
    /// <summary>
    /// The account confirmation email definition 
    /// </summary>
    public class AccountConfirmationEmail : EmailBase
    {
        private readonly IStringLocalizer<EmailResource> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountConfirmationEmail"/> class.
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="replaceValues"></param>
        public AccountConfirmationEmail(IStringLocalizer<EmailResource> localizer, List<KeyValuePair<string, string>> replaceValues) :
            base(replaceValues)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Get a raw email body
        /// </summary>
        /// <returns>Raw email body</returns>
        public override string GetRawEmailBody()
        {
            return _localizer["Click the link below to confirm your account."];
        }

        /// <summary>
        /// Get an email subject
        /// </summary>
        /// <returns>Email subject</returns>
        public override string GetSubject()
        {
            return _localizer["Account Confirmation"];
        }
    }
}
