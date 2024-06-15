using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;

namespace Mshrm.Studio.Auth.Api.Middleware
{
    /// <summary>
    /// Configures the HttpContext by assigning IdentityServerOrigin.
    /// </summary>
    public class IdentityOriginSettingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _publicFacingUri;

        public IdentityOriginSettingMiddleware(RequestDelegate next, string publicFacingUri)
        {
            _publicFacingUri = publicFacingUri;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.RequestServices.GetRequiredService<IServerUrls>().Origin = _publicFacingUri;

            await _next(context);
        }
    }

    //app.UseMiddleware<PublicFacingUrlMiddleware>(Configuration[IdentityServerPublicFacingUri]);
}
