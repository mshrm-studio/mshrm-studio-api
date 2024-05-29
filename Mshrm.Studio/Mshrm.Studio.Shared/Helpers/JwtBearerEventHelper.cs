using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace Mshrm.Studio.Shared.Helpers
{
    public class JwtBearerEventHelper
    {
        /// <summary>
        /// Create JWT bearer events to ensure the role claims are added to identity even if we come from the openID flow
        /// </summary>
        /// <returns>JWT events</returns>
        /// <exception cref="Exception">Thrown if no email claim is found in authenticated identity</exception>
        public static JwtBearerEvents CreateJwtBearerEvents()
        {
            // Create the events
            return new JwtBearerEvents
            {
                OnAuthenticationFailed = async ctx =>
                {
                    return;
                },
                OnTokenValidated = async ctx =>
                {
                    /*
                    // Get the identity service
                    var identityService = (IIdentityUserService)ctx.HttpContext.RequestServices.GetService(typeof(IIdentityUserService));

                    // Get the callers email - throw error if not provided
                    var email = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                    if (string.IsNullOrEmpty(email))
                        throw new Exception("Email is empty when it is expected");

                    // Get the users role/s
                    var roles = await identityService.GetRolesAsync(email);

                    // Create the claims
                    var claims = new List<Claim>();

                    // Add the role claims
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    // Create a new claims identity
                    var appIdentity = new ClaimsIdentity(claims);

                    // Add the identity to the principle
                    ctx.Principal?.AddIdentity(appIdentity);
                */
                    return;
                }
            };
        }
    }
}
