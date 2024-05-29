using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Mshrm.Studio.Api.Clients.Bases;
using System.Net.Http.Headers;

namespace Mshrm.Studio.Api.Clients.Pricing
{
    public partial class PricesClient : BaseClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PricesClient(string baseUrl, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
