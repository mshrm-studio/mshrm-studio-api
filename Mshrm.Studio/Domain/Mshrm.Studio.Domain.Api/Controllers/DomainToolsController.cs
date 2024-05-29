using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Domain.Api.Models.Dtos.Users;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Domain.Api.Models.Dtos.ContactForms;
using Mshrm.Studio.Shared.Models.Dtos;
using Microsoft.Data.SqlClient;
using Mshrm.Studio.Shared.Models.Pagination;
using SortOrder = Mshrm.Studio.Shared.Models.Pagination.SortOrder;
using Mshrm.Studio.Domain.Api.Models.Dtos.Tools;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.CQRS.Tools.Queries;
using MediatR;

namespace Mshrm.Studio.Domain.Api.Controllers
{
    /// <summary>
    /// Tools API
    /// </summary>
    [ApiController]
    [Route("api/v1/tools")]
    public class DomainToolsController : ControllerBase
    {
        private readonly ILogger<DomainToolsController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToolsController"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        public DomainToolsController(ILogger<DomainToolsController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;

            _mediator = mediator;
        }

        /// <summary>
        /// Get a tool by Guid
        /// </summary>
        /// <param name="guid">The tool</param>
        /// <returns>A tool if exists</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [Route("guid/{guid}")]
        public async Task<ActionResult<ToolDto>> GetToolByGuidAsync([FromRoute] Guid guid)
        {
            // Get tool
            var tool = await _mediator.Send<Tool>(new GetToolByGuidQuery(){ GuidId = guid }, Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<ToolDto>(tool));
        }

        /// <summary>
        /// Get a page of tools
        /// </summary>
        /// <param name="searchTerm">A search term (message)</param>
        /// <param name="name">The tools name</param>
        /// <param name="toolType">The tools type</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <returns>A page of tools</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResultDto<ToolDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<PageResultDto<ToolDto>>> GetToolsAsync([FromQuery] string? searchTerm, [FromQuery] string? name, [FromQuery] ToolType? toolType,
            [FromQuery] string? orderProperty = "rank", [FromQuery] Order order = Order.Descending, 
            [FromQuery] uint pageNumber = 1, [FromQuery] uint perPage = 30)
        {
            // Get tools
            var tools = await _mediator.Send<PagedResult<Tool>>(new GetToolsPagedQuery()
            {
                SearchTerm = searchTerm,
                Name = name,
                ToolType = toolType,
                Order = order,
                OrderProperty = orderProperty,
                PageNumber = pageNumber,
                PerPage = perPage
            }, Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<PageResultDto<ToolDto>>(tools));
        }

        /// <summary>
        /// Create a new tool
        /// </summary>
        /// <param name="dto">The tool to create</param>
        /// <returns>The new tool</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status201Created)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("")]
        public async Task<ActionResult<ToolDto>> CreateToolAsync([FromBody] CreateToolDto dto)
        {
            // Create tool
            var tool = await _mediator.Send<Tool>(_mapper.Map<CreateToolCommand>(dto), Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<ToolDto>(tool));
        }

        /// <summary>
        /// Update a new tool
        /// </summary>
        /// <param name="toolGuidId"></param>
        /// <param name="dto">The tool to update</param>
        /// <returns>The tool</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status201Created)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
        [Route("{toolGuidId}")]
        public async Task<ActionResult<ToolDto>> UpdateToolAsync([FromRoute] Guid toolGuidId, [FromBody] UpdateToolDto dto)
        {
            // Map request command
            var command = _mapper.Map<UpdateToolCommand>(dto);
            command.GuidId = toolGuidId;

            // Send request to update tool
            var tool = await _mediator.Send<Tool>(command, Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<ToolDto>(tool));
        }
    }
}
