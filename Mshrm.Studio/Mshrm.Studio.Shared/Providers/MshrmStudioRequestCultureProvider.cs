using Azure.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Mshrm.Studio.Shared.Providers
{
    public class MshrmStudioRequestCultureProvider : RequestCultureProvider
    {
        private readonly RequestLocalizationOptions _localizationOptions;

        public MshrmStudioRequestCultureProvider(RequestLocalizationOptions localizationOptions)
        {
            _localizationOptions = localizationOptions;
        }

        public override async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            await Task.Yield();

            // Get from header
            var locales = httpContext.Request.Headers.FirstOrDefault(x => x.Key == "Accept-Language");
            var localesValue = locales.Value.ToString();
            if (localesValue == null)
                return new ProviderCultureResult(_localizationOptions?.DefaultRequestCulture.Culture.IetfLanguageTag);

            var localesList = localesValue.Split(','); 
            var locale = localesList.FirstOrDefault();

            var supportedLocales = _localizationOptions?.SupportedCultures?.Select(x => x.IetfLanguageTag.ToLower())?.ToList();
            if (supportedLocales == null)
                return new ProviderCultureResult(_localizationOptions?.DefaultRequestCulture.Culture.IetfLanguageTag);

            var match = supportedLocales.FirstOrDefault(x => x == locale?.ToLower());
            if (!string.IsNullOrEmpty(match))
            {
                return new ProviderCultureResult(match);
            }

            if (locale == null || locale.IndexOf("-") >= 0)
            {
                var firstPartOfLocale = locale?.Substring(0, locale.IndexOf("-"))?.ToLower();
                match = supportedLocales.FirstOrDefault(x => x == firstPartOfLocale);
                if (!string.IsNullOrEmpty(match))
                {
                    return new ProviderCultureResult(match);
                }
            }

            return new ProviderCultureResult(_localizationOptions?.DefaultRequestCulture.Culture.IetfLanguageTag);
        }

        #region Helpers

   
        #endregion
    }
}
