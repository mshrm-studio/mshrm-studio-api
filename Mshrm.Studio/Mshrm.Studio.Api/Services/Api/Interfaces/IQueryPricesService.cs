using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Prices;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryPricesService
    {
        /// <summary>
        /// Get the latest prices for supported currencies
        /// </summary>
        /// <param name="pricingProviderType">An optional pricing provider</param>
        /// <param name="currencyType">An optional currency type</param>
        /// <param name="baseCurrency">An optional base currencie ie. output to what</param>
        /// <param name="symbols">Filter by supported currencies</param>
        /// <param name="cancellaationToken">Stopping token</param>
        /// <returns>A list of latest prices</returns>
        Task<List<PriceDto>> GetLatestPricesAsync(PricingProviderType? pricingProviderType, CurrencyType? currencyType, string baseCurrency, List<string>? symbols, CancellationToken cancellaationToken);
    }
}
