﻿using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Application.Services.Providers;
using Mshrm.Studio.Pricing.Domain.AssetPrices.Queries;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Providers
{
    public class GetProviderPricesQuerysHandler : IRequestHandler<GetProviderPricesQuery, List<PricePair>>
    {
        private readonly AssetPriceServiceResolver _assetPriceServiceResolver;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProviderPricesQuerysHandler"/> class.
        /// </summary>
        /// <param name="assetPriceServiceResolver"></param>
        /// <param name="tracer"></param>
        public GetProviderPricesQuerysHandler(AssetPriceServiceResolver assetPriceServiceResolver, ITracer tracer)
        {
            _assetPriceServiceResolver = assetPriceServiceResolver;

            _tracer = tracer;
        }

        /// <summary>
        /// Get the latest prices for symbols
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>Latest prices</returns>
        public async Task<List<PricePair>> Handle(GetProviderPricesQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetProviderPricesQuerysHandler").StartActive(true))
            {
                var provider = _assetPriceServiceResolver(query.ProviderType);

                return await provider.GetPricesAsync(query.ProviderCurrencySymbols);
            }
        }
    }
}
