using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Auth.Application.Options;

namespace Mshrm.Studio.Auth.Api.Middleware
{
    /// <summary>
    /// Configures the HttpContext by assigning IdentityServerOrigin.
    /// </summary>
    public class IdentityOriginSettingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IdentityOriginOptions _options;

        public IdentityOriginSettingMiddleware(RequestDelegate next, IOptions<IdentityOriginOptions> options)
        {
            _options = options.Value;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.RequestServices.GetRequiredService<IServerUrls>().Origin = _options.IdentityServerPublicFacingUri;

            await _next(context);
        }
    }

    //app.UseMiddleware<PublicFacingUrlMiddleware>(Configuration[IdentityServerPublicFacingUri]);
}
