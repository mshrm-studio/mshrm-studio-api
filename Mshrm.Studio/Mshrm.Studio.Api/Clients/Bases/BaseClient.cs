using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Api.Clients.Bases
{
    public abstract class BaseClient
    {
        /// <summary>
        /// Takes bearer token from initial request and propegates it to the down service client called
        /// </summary>
        /// <param name="request">The down service client request</param>
        /// <param name="httpContextAccessor">The current service request/response context</param>
        internal void AddAuthorizationHeader(HttpRequestMessage request, IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext.Request.Headers.Authorization.Any())
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext?.Request?.Headers?.Authorization.ToString()?.Substring(7));
            }
        }

        /// <summary>
        /// Takes culture from initial request and propegates it to the down service client called
        /// </summary>
        /// <param name="request">The down service client request</param>
        /// <param name="httpContextAccessor">The current service request/response context</param>
        internal void AddCulture(HttpRequestMessage request, IHttpContextAccessor httpContextAccessor)
        {
            // Get current culture and propagate
            var rqf = httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = rqf.RequestCulture.Culture.ToString();
            
            request.Headers.Add("Accept-Language", culture);
        }
    }
}
