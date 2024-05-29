using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Domain.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Infrastructure.Factories
{
    public class CurrencyFactory : ICurrencyFactory
    {
        /// <summary>
        /// Create a new currency
        /// </summary>
        /// <param name="providerType">The provider to import currency from</param>
        /// <param name="currencyType">The type of currency ie. Fiat</param>
        /// <param name="name">The name of the currency</param>
        /// <param name="symbol">A symbol for the currency</param>
        /// <param name="symbolNative">The display synbol ie. $ for US dollars</param>
        /// <param name="description">A description</param>
        /// <param name="logoGuidId">A logo</param>
        /// <returns>A new currency</returns>
        public Currency CreateCurrency(PricingProviderType providerType, CurrencyType currencyType, string name, string symbol, string symbolNative, string? description, Guid? logoGuidId)
        {
            return new Currency(providerType, currencyType, name, symbol, symbolNative, description, logoGuidId);
        }
    }
}
