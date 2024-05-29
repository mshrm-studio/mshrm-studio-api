using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Api.Clients.Localization;
using System.Collections;
using System.Globalization;

namespace Mshrm.Studio.Api.Services.Localization
{
    public class ApiResourceStringLocalizer : IStringLocalizer
    {
        private readonly ILocalizationClient _localizationClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly RequestLocalizationOptions _localizationOptions;

        public ApiResourceStringLocalizer(ILocalizationClient localizationClient, IHttpContextAccessor contextAccessor ,IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _localizationClient = localizationClient;
            _contextAccessor = contextAccessor;

            _localizationOptions = localizationOptions.Value;

            // default
            ResourceSet =".CommonResources";
        }

        /// <summary>
        /// Default ResourceSetName if no Template type is provided
        /// </summary>
        public string ResourceSet { get; set; }

        /// <summary>
        /// Returns all strings
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var resources = _localizationClient.GetLocalizationResourcesAsync(null, null, null).GetAwaiter().GetResult();
            if (resources != null)
            {
                foreach (var resource in resources)
                {
                    var key = resource.Name as string;
                    var value = resource.Value as string;

                    var localizedString = new LocalizedString(key, value);
                    yield return localizedString;
                }
            }
        }

        LocalizedString IStringLocalizer.this[string name]
        {
            get
            {
                // Retrieves the requested culture
                var matchingResource = LocalizeAsync(name).GetAwaiter().GetResult();

                // Return localized string
                return new LocalizedString(name, matchingResource ?? name);
            }
        }

        LocalizedString IStringLocalizer.this[string name, params object[] arguments]
        {
            get
            {
                // Retrieves the requested culture
                var matchingResource = LocalizeAsync(name).GetAwaiter().GetResult();

                // Format value
                var value = string.Format(matchingResource ?? name, arguments);

                // Return localized string
                return new LocalizedString(name, value ?? name);
            }
        }

        #region Helpers

        public async Task<string?> LocalizeAsync(string name)
        {
            var requestCultureFeature = _contextAccessor.HttpContext.Features.GetRequiredFeature<IRequestCultureFeature>();
            var culture = requestCultureFeature.RequestCulture.Culture;
            var defaultCulture = _localizationOptions.DefaultRequestCulture.Culture;

            var matchingResources = await _localizationClient.GetLocalizationResourcesAsync(null, culture.Name, name);
            var matchingResource = matchingResources.FirstOrDefault();

            // If no match then use default locale
            if(matchingResource == null)
            {
                matchingResources = await _localizationClient.GetLocalizationResourcesAsync(null, defaultCulture.Name, name);
                matchingResource = matchingResources.FirstOrDefault();
            }    

            return matchingResource?.Value;
        }

        #endregion
    }
}
