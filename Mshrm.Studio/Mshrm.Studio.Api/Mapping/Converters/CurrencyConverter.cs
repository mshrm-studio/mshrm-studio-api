using AutoMapper;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Currencies;
using System.Runtime.Intrinsics;

namespace Mshrm.Studio.Api.Mapping.Converters
{
    /// <summary>
    /// For the converstion of currencyDTO to pricingCurrencyDTO
    /// </summary>
    public class CurrencyConverter : ITypeConverter<CurrencyDto, CurrencyResponseDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyConverter"/> class.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public CurrencyConverter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Convert a CurrencyDto to a PricingCurrencyDto
        /// </summary>
        /// <param name="parent">The CurrencyDto</param>
        /// <param name="target">The PricingCurrencyDto</param>
        /// <param name="context">The request context</param>
        /// <returns>A PricingCurrencyDto</returns>
        public CurrencyResponseDto Convert(
            CurrencyDto parent,
            CurrencyResponseDto target,
            ResolutionContext context)
        {
            target = new CurrencyResponseDto();

            // Build the url
            var url = (parent.LogoGuidId.HasValue) ? $"https://{_httpContextAccessor.HttpContext?.Request.Host}/api/v1/files/{parent.LogoGuidId}" : null;

            // Set the rest of the properties
            target.CreatedDate = parent.CreatedDate;
            target.Description = parent.Description;
            target.Active = parent.Active;
            target.CurrencyType = parent.CurrencyType;
            target.ProviderType = parent.ProviderType;
            target.GuidId = parent.GuidId;
            target.Name = parent.Name;
            target.Symbol = parent.Symbol;
            target.SymbolNative = parent.SymbolNative;
            target.logoGuidId = parent.LogoGuidId;
            target.LogoUrl = url;

            // Return mapped target
            return target;
        }
    }
}
