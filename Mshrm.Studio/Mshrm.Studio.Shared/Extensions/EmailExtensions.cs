using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Extensions
{
    public static class EmailExtensions
    {
        /// <summary>
        /// Uses the same validation that the Mail library uses to validate emails when sending
        /// </summary>
        /// <param name="email">The email to validate</param>
        /// <returns>True if the email is valid</returns>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith("."))
                return false;

            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
