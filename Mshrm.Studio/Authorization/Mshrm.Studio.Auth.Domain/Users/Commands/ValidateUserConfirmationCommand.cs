﻿using MediatR;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Extensions;
using System.Reflection.Metadata.Ecma335;

namespace Mshrm.Studio.Auth.Domain.User.Commands
{
    /// <summary>
    /// For confirming users
    /// </summary>
    public class ValidateUserConfirmationCommand : IRequest<bool>
    {
        /// <summary>
        /// The email to validate against
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The token to validate against
        /// </summary>
        public required string ConfirmationToken { get; set; }
    }
}
