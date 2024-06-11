using MediatR;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairHistories.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Jobs.Interfaces;
using Mshrm.Studio.Pricing.Domain.ExchangePricingPairs;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.ExchangePricePair
{
    /// <summary>
    /// Create a price pair
    /// </summary>
    public class ImportPricesCommandHandler : IRequestHandler<ImportPricesCommand, bool>
    {
        private readonly IJobsService _jobsService;
        private readonly ILogger<ImportPricesCommandHandler> _logger;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportPricesCommandHandler"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tracer"></param>
        public ImportPricesCommandHandler(IJobsService jobsService, ILogger<ImportPricesCommandHandler> logger, ITracer tracer)
        {
            _jobsService = jobsService;

            _tracer = tracer;
            _logger = logger;
        }

        /// <summary>
        /// Create or replace existing pricing pairs in database
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The prices added/updated</returns>
        public async Task<bool> Handle(ImportPricesCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("ImportPricesAsync").StartActive(true))
            {
                return await _jobsService.ImportProviderPairsAsync(command.PricingProviderType);
            }
        }
    }
}
