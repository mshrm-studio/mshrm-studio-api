using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Domain.ExchangePricingPairs
{
    public interface IExchangePricingPairFactory
    {
        /// <summary>
        /// Create a new exchange pricing pair
        /// </summary>
        /// <param name="baseCurrencyId">The base currrency</param>
        /// <param name="currencyId">The currency to create price for</param>
        /// <param name="price">The price against base currency</param>
        /// <param name="marketCap">The market cap</param>
        /// <param name="volume">The volume</param>
        /// <param name="pricingProviderType">A pricing provider type (imported with)</param>
        /// <returns>A price for a currency</returns>
        public ExchangePricingPair CreateExchangePricingPair(int baseCurrencyId, int currencyId, decimal price, decimal? marketCap, decimal? volume, PricingProviderType pricingProviderType);
    }
}
