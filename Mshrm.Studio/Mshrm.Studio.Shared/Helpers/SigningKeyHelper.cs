using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Mshrm.Studio.Shared.Helpers
{
    public class SigningKeyHelper
    {
        /// <summary>
        /// Get open ID security keys from a ../well-known endpoint
        /// </summary>
        /// <param name="openIdConfigUri">The endpoint to get configuration from</param>
        /// <returns>Signing keys</returns>
        public static async Task<List<SecurityKey>> GetOpenIdSecurityKeysAsync(string openIdConfigUri)
        {
            // Create the config manager and init with uri to get config from
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(openIdConfigUri, new OpenIdConnectConfigurationRetriever());

            // Get the config from the endpoint
            var openidconfig = await configManager.GetConfigurationAsync();

            // Return the signing keys
            return openidconfig.SigningKeys?.ToList() ?? new List<SecurityKey>();
        }

        /// <summary>
        /// Create a security key from a secret string
        /// </summary>
        /// <param name="secret">The secret to turn to a security key</param>
        /// <returns>A security key</returns>
        public static SecurityKey CreateSigningKey(string secret)
        {
            // Get the secret in bytes
            var secretBytes = Encoding.UTF8.GetBytes(secret);

            // Create the key to generate singing key with
            var authSigningKey = new SymmetricSecurityKey(secretBytes);

            // Create the signing credentials using HmacSHA256
            var signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

            // Return the key
            return signingCredentials.Key;
        }

        /// <summary>
        /// Get security keys from openID + any new ones to manually create
        /// </summary>
        /// <param name="openIdConfigUris">OpenId well-known endpoint/s</param>
        /// <param name="additionalSigningKeys">Any additional signing keys to add ie. from appication</param>
        /// <returns>A list of signing keys for the application to decode JWT with</returns>
        public static async Task<List<SecurityKey>> GetSigningKeysAsync(List<string> openIdConfigUris, params string[] additionalSigningKeys)
        {
            var signingKeys = new List<SecurityKey>();

            // Get the signing keys for OIDC
            if ((openIdConfigUris?.Any() ?? false))
            {
                // Get openId configs
                foreach (var openIdConfigUri in openIdConfigUris)
                {
                    // Get security keys from config + add to list
                    var openIdSigningKeys = await SigningKeyHelper.GetOpenIdSecurityKeysAsync(openIdConfigUri);
                    if ((openIdSigningKeys?.Any() ?? false))
                        signingKeys.AddRange(openIdSigningKeys);
                }
            }

            // Get the signing key/s for our application/s
            if ((additionalSigningKeys?.Any() ?? false))
            {
                foreach (var additionalSigningKey in additionalSigningKeys)
                {
                    // Create key and add to list
                    var newKey = SigningKeyHelper.CreateSigningKey(additionalSigningKey);
                    if(newKey != null)
                        signingKeys.Add(newKey);
                }
            }

            return signingKeys;
        }
    }
}
