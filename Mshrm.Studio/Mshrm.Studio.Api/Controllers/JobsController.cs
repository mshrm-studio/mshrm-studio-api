using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Controllers.Bases;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Controllers
{
    /// <summary>
    /// Jobs API
    /// </summary>
    [ApiController]
    [FormatFilter]
    [Route("api/v1/jobs")]
    public class JobsController : MshrmStudioBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICreateJobService _createJobService;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsController"/> class.
        /// </summary>
        /// <param name="createJobService"></param>
        /// <param name="domainUserClient"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="mapper"></param>
        public JobsController(ICreateJobService createJobService, IDomainUserClient domainUserClient, IHttpContextAccessor contextAccessor,
            IMapper mapper) : base(domainUserClient, contextAccessor)
        {
            _createJobService = createJobService;

            _mapper = mapper;
        }

        /// <summary>
        /// Run price import job
        /// </summary>
        /// <param name="pricingProviderType">The provider to import prices for</param>
        /// <returns>If the job was successful or not</returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("import-prices/{pricingProviderType}")]
        public async Task<ActionResult<bool>> ImportPricesAsync([FromRoute] PricingProviderType pricingProviderType)
        {
            var jobRun = await _createJobService.ImportPricesAsync(pricingProviderType);

            return Ok(jobRun);
        }
    }
}
