using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Queries;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Jobs.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Mshrm.Studio.Pricing.Application.Services.Providers;
using Mshrm.Studio.Pricing.Domain.AssetPrices.Queries;
using Mshrm.Studio.Shared.Models.Pagination;
using Newtonsoft.Json;
using System.Text;

namespace Mshrm.Studio.Pricing.Api.Services.Jobs
{
    public class JobsService : IJobsService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<JobsService> _logger;

        public JobsService(IMediator mediator, ILogger<JobsService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Helpers

        /// <summary>
        /// Pull in asset price updates
        /// </summary>
        /// <param name="type">Provider type</param>
        /// <returns>An async task</returns>
        public async Task<bool> ImportProviderAssetsAsync(PricingProviderType type)
        {
            try
            {
                _logger.LogInformation($"Running import for {type.ToString()} at {DateTime.UtcNow}");

                // Get assets that use the prices
                var providerAssets = await _mediator.Send<PagedResult<Asset>>(new GetPagedAssetsQuery() { PricingProviderType = type, PageNumber=0, PerPage=999999, OrderProperty="CreatedDate", Order = Shared.Enums.Order.Descending });
                var providerCurrencySymbols = providerAssets.Results.Select(y => y.Symbol).ToList();

                _logger.LogInformation($"Getting prices for {string.Join(',', providerCurrencySymbols)} at {DateTime.UtcNow}");

                var prices = await _mediator.Send<List<PricePair>>(new GetProviderPricesQuery() { ProviderCurrencySymbols = providerCurrencySymbols, ProviderType = type });

                // Add to database
                await _mediator.Send<List<AssetPrice>>(new CreateOrReplaceAssetPricesCommand()
                {
                    Prices = prices,
                    PricingProviderType = type
                }, CancellationToken.None);

                _logger.LogInformation($"Finishing import for {type.ToString()} at {DateTime.UtcNow}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Job for provider: {type} failed");
                _logger.LogCritical(ex.StackTrace);

                return false;
            }
        }

        #endregion
    }
}
