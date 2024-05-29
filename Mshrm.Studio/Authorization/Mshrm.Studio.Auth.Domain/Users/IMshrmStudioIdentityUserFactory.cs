using Mshrm.Studio.Auth.Api.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Domain.Users
{
    public interface IMshrmStudioIdentityUserFactory
    {
        /// <summary>
        /// Create a new Mshrm Studio Identity user
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="userName">The username</param>
        /// <param name="emailConfirmed">If account confirmed</param>
        /// <returns>A new user</returns>
        public MshrmStudioIdentityUser CreateMshrmStudioIdentityUser(string email, string userName, bool emailConfirmed);
    }
}
