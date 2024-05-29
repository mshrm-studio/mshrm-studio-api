using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Mshrm.Studio.Api.Clients.Bases;
using System.Net.Http.Headers;

namespace Mshrm.Studio.Api.Clients.Email
{
    public partial class EmailClient : BaseClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailClient(string baseUrl, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
