using Duende.IdentityServer.Models;

namespace Mshrm.Studio.Auth.Web.Config
{
    public class Configuration
    {
        public static IEnumerable<Client> GetClients()
        {
            var secret = "client_secret".Sha256();

            return new List<Client>
            {
                new Client
                {
                    Enabled = true,
                    ClientId = "client_id",
                    ClientSecrets = { new Secret(secret) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "https://localhost:7127/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:7127/signout-callback-oidc" },
                    AllowedScopes = { "openid", "profile", "api1" },

                    AllowOfflineAccess = true
                }
            };
        }
    }
}
