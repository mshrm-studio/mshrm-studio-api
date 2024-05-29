using Microsoft.AspNetCore.Http;
using Mshrm.Studio.Api.Clients.Bases;
using System.Net.Http.Headers;

namespace Mshrm.Studio.Api.Clients.Auth
{
    public partial class IdentityUserClient : BaseClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityUserClient(string baseUrl, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            BaseUrl = baseUrl;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            base.AddAuthorizationHeader(request, _httpContextAccessor);
            base.AddCulture(request, _httpContextAccessor);
        }
    }
}
