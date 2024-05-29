using Mshrm.Studio.Api.Clients.Bases;
using System.Net.Http;

namespace Mshrm.Studio.Api.Clients.Domain
{
    public partial class DomainContactFormClient : BaseClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DomainContactFormClient(string baseUrl, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
