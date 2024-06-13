using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Shared.Models.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Services
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IOptions<JwtOptions> _options;

        public IdentityProfileService(IOptions<JwtOptions> options) 
        { 
            _options = options;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = new List<Claim>
            {
                new Claim("aud", _options.Value.Audience, ClaimValueTypes.String)
            };

            context.IssuedClaims.AddRange(context.Subject.Claims);
            context.IssuedClaims.AddRange(claims);

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
