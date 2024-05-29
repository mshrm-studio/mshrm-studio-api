using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Helpers
{
    public class IssuerHelper
    {
        /// <summary>
        /// Validate issuers of tokens
        /// </summary>
        /// <param name="issuer">The issuer attempting to validate</param>
        /// <param name="securityToken">The token provided to validate issuer for</param>
        /// <param name="validationParameters">Validate params defined on setup</param>
        /// <returns>A valid issuer (if any)</returns>
        /// <exception cref="SecurityTokenInvalidIssuerException">Thrown if valid issuer is not matched</exception>
        public static string ValidateIssuer(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            // We try to validate if is a JWT
            if (securityToken is JsonWebToken jwt)
            {
                // Get all the valid issuers
                var validIssuerSet = (validationParameters.ValidIssuers ?? Enumerable.Empty<string>())
                    .Append(validationParameters.ValidIssuer)
                    .Where(i => !string.IsNullOrEmpty(i))
                    .ToList();

                // If comes from microsoft (tenantId) we check if we have a matching item to validate against
                if (jwt.TryGetClaim("tid", out var value) &&
                    value is Claim tokenTenantId)
                {
                    // Checks all whitelisted issuers and if one is proveded ie. xxxx/[tenantid]/xxx -> xxxx/tokenTenantId/xxx then all Microsoft tenants are valid
                    if (validIssuerSet.Any(i => i.Replace("[tenantid]", tokenTenantId.Value) == issuer))
                        return issuer;
                }
                // If comes from elsewhere, check normally against issuers
                else if (validIssuerSet.Any(i => i == issuer))
                {
                    return issuer;
                }
            }

            // Check issuer
            if (validationParameters.ValidIssuer == issuer)
            {
                return issuer;
            }

            // Recreate the exception that is thrown by default when issuer validation fails
            var validIssuer = validationParameters.ValidIssuer ?? "null";
            var validIssuers = validationParameters.ValidIssuers == null ? "null" : !validationParameters.ValidIssuers.Any() ? 
                "empty" : string.Join(", ", validationParameters.ValidIssuers);

            string errorMessage = FormattableString.Invariant(
                $"IDX10205: Issuer validation failed. Issuer: '{issuer}'. Did not match: validationParameters.ValidIssuer: '{validIssuer}' or validationParameters.ValidIssuers: '{validIssuers}'.");

            // Throw issuer invalid error as would normally be thrown
            throw new SecurityTokenInvalidIssuerException(errorMessage)
            {
                InvalidIssuer = issuer
            };
        }
    }
}
