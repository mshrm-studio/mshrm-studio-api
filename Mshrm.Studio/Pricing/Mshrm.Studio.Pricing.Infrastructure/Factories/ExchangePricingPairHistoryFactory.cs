using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Domain.ExchangePricingPairHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Infrastructure.Factories
{
    public class ExchangePricingPairHistoryFactory : IExchangePricingPairHistoryFactory
    {
        /// <summary>
        /// Create an exchange pricing pair history record
        /// </summary>
        /// <param name="exchangePricingPairId">The pair that was updated/created</param>
        /// <param name="pricingProviderType">The pricing provider used for import</param>
        /// <param name="oldPrice">The old price</param>
        /// <param name="newPrice">The new price</param>
        /// <param name="oldMarketCap">The old market cap</param>
        /// <param name="newMarketCap">The new market cap</param>
        /// <param name="oldVolume">The old volume</param>
        /// <param name="newVolume">The new volume</param>
        /// <returns>A pricing pair history</returns>
        public ExchangePricingPairHistory CreateExchangePricingPairHistory(int exchangePricingPairId, PricingProviderType pricingProviderType, decimal oldPrice, decimal newPrice,
            decimal? oldMarketCap, decimal? newMarketCap, decimal? oldVolume, decimal? newVolume)
        {
            return new ExchangePricingPairHistory(exchangePricingPairId, pricingProviderType, oldPrice, newPrice, oldMarketCap, newMarketCap, oldVolume, newVolume);
        }
    }
}
