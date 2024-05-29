using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Commands
{
    public class UpdateSupportedCurrencyCommand : IRequest<Currency>
    {
        /// <summary>
        /// The currency name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The native symbol ie. USD -> $
        /// </summary>
        public required string SymbolNative { get; set; }

        /// <summary>
        /// The description for the currency
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The provider for currency import
        /// </summary>
        public PricingProviderType ProviderType { get; set; }

        /// <summary>
        /// The type of currency
        /// </summary>
        public CurrencyType CurrencyType { get; set; }

        /// <summary>
        /// The supported currencies logo GUID
        /// </summary>
        public Guid? LogoGuidId { get; set; }

        /// <summary>
        /// The currency to update
        /// </summary>
        public Guid CurrencyId { get; set; }
    }
}
