using Mshrm.Studio.Auth.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Domain.Application.Dtos.Users
{
    public class UpdateUserDto
    {
        /// <summary>
        /// The users first name
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public required string LastName { get; set; }
    }
}
