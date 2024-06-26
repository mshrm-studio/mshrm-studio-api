﻿using MediatR;
using Mshrm.Studio.Auth.Domain.Clients.Enums;
using Mshrm.Studio.Auth.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Entities;

namespace Mshrm.Studio.Auth.Domain.ApiResources.Commands
{
    public class CreateApiScopeCommand : IRequest<ApiScope>
    {
        public string Name { get; set; }
        public List<string> Scopes { get; set; } = new List<string>();
    }
}
