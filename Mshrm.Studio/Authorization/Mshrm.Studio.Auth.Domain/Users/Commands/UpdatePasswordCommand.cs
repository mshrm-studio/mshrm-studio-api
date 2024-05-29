﻿using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using System.Threading;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using MediatR;
using Mshrm.Studio.Auth.Api.Models.Entities;

namespace Mshrm.Studio.Auth.Domain.User.Commands
{
    /// <summary>
    /// For updating a users password
    /// </summary>
    public class UpdatePasswordCommand : IRequest
    {
        /// <summary>
        /// The email
        /// </summary>
        public required string Email { get; set; }
    }
}
