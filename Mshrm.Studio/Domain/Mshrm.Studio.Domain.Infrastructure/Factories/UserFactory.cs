using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Domain.Infrastructure.Factories
{
    public class UserFactory : IUserFactory
    {
        /// <summary>
        /// Create a new user 
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="active">If the user is active or not</param>
        /// <param name="firstName">The first name of the user</param>
        /// <param name="lastName">The last name of the user</param>
        /// <param name="ip">The IP of the user</param>
        /// <returns>A new user</returns>
        public User CreateUser(string email, string firstName, string lastName, string? ip, bool active)
        {
            return new User(email.ToLower().Trim(), firstName.Trim(), lastName.Trim(), ip, active);
        }
    }
}
