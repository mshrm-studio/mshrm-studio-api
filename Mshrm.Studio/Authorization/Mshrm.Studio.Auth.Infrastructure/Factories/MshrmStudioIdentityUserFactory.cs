using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Pocos;
using Mshrm.Studio.Auth.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Infrastructure.Factories
{
    public class MshrmStudioIdentityUserFactory : IMshrmStudioIdentityUserFactory
    {
        /// <summary>
        /// Create a new Mshrm Studio Identity user
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="userName">The username</param>
        /// <param name="emailConfirmed">If account confirmed</param>
        /// <returns>A new user</returns>
        public MshrmStudioIdentityUser CreateMshrmStudioIdentityUser(string email, string userName, bool emailConfirmed)
        {
            return new MshrmStudioIdentityUser(email.ToLower().Trim(), userName.ToLower().Trim(), emailConfirmed);
        }
    }
}
