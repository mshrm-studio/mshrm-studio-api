using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Mshrm.Studio.Pricing.Api.Extensions;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Queries;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Prices;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Mshrm.Studio.Pricing.Application.Dtos.Prices;
using Mshrm.Studio.Pricing.Domain.ExchangePricingPairHistories.Queries;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using Newtonsoft.Json;
using System.Text;

namespace Mshrm.Studio.Pricing.Api.Controllers
{
    /// <summary>
    /// Jobs API
    /// </summary>
    [ApiController]
    [Route("api/v1/jobs")]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public JobController(IMediator mediator, ILogger<JobController> logger, IMapper mapper)
        {
            _mediator = mediator;

            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Import prices by provider
        /// </summary>
        /// <param name="pricingProviderType">The provider to import prices for</param>
        /// <returns>An action result</returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [Route("import-prices/{pricingProviderType}")]
        public async Task<ActionResult<bool>> ImportPricesAsync([FromRoute] PricingProviderType pricingProviderType)
        {
            var jobRun = await _mediator.Send<bool>(new ImportPricesCommand()
            {
                PricingProviderType = pricingProviderType
            });

            return Ok(jobRun);
        }
    }
}
