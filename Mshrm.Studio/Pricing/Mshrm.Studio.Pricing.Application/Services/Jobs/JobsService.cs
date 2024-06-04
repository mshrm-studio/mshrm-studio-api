using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Queries;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Jobs.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Mshrm.Studio.Pricing.Application.Services.Providers;
using Newtonsoft.Json;
using System.Text;

namespace Mshrm.Studio.Pricing.Api.Services.Jobs
{
    public class JobsService : IJobsService
    {
        private readonly AssetPriceServiceResolver _assetPriceServiceResolver;
        private readonly IDistributedCache _distributedCache;
        private readonly IMediator _mediator;

        private readonly ILogger<JobsService> _logger;

        public JobsService(IMediator mediator, AssetPriceServiceResolver assetPriceServiceResolver, IDistributedCache distributedCache,
            ILogger<JobsService> logger)
        {
            _mediator = mediator;

            _assetPriceServiceResolver = assetPriceServiceResolver;
            _distributedCache = distributedCache;

            _logger = logger;
        }

        #region Helpers

        /// <summary>
        /// Pull in asset pair updates
        /// </summary>
        /// <param name="type">Provider type</param>
        /// <returns>An async task</returns>
        public async Task ImportProviderPairsAsync(PricingProviderType type)
        {
            try
            {
                // Get provider
                var provider = _assetPriceServiceResolver(type);

                // Get assets that use the prices
                var providerCurrencies = await _mediator.Send<List<Asset>>(new GetAssetsQuery() { PricingProviderType = type });
                var providerCurrencySymbols = providerCurrencies.Select(y => y.Symbol).ToList();

                // Get prices
                var prices = await provider.GetPricesAsync(providerCurrencySymbols);

                // Add to database
                await _mediator.Send<List<ExchangePricingPair>>(new CreateOrReplacePricingPairsCommand()
                {
                    Prices = prices,
                    PricingProviderType = type
                }, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Job for provider: {type} failed");
                _logger.LogCritical(ex.StackTrace);
            }
        }

        #endregion
    }
}
