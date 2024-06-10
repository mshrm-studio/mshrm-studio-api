using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Controllers.Bases;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Mshrm.Studio.Api.Models.Dtos.Tools;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using Order = Mshrm.Studio.Shared.Enums.Order;

namespace Mshrm.Studio.Api.Controllers
{
    /// <summary>
    /// Tools API
    /// </summary>
    [ApiController]
    [FormatFilter]
    [Route("api/v1/tools")]
    public class ToolsController : MshrmStudioBaseController
    {
        private readonly IMapper _mapper;
        private readonly IQueryToolsService _queryToolsService;
        private readonly ICreateToolsService _createToolsService;
        private readonly IUpdateToolsService _updateToolsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolsController"/> class.
        /// </summary>
        /// <param name="domainUserClient"></param>
        /// <param name="queryToolsService"></param>
        /// <param name="createToolsService"></param>
        /// <param name="updateToolsService"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="mapper"></param>
        public ToolsController(IDomainUserClient domainUserClient, IQueryToolsService queryToolsService, ICreateToolsService createToolsService, IUpdateToolsService updateToolsService,
            IHttpContextAccessor contextAccessor, IMapper mapper) : base(domainUserClient, contextAccessor)
        {
            _queryToolsService = queryToolsService;
            _createToolsService = createToolsService;
            _updateToolsService = updateToolsService;

            _mapper = mapper;
        }

        /// <summary>
        /// Get a tool by Guid
        /// </summary>
        /// <param name="guid">The tool</param>
        /// <returns>A tool if exists</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ToolResponseDto), StatusCodes.Status200OK)]
        [Route("guid/{guid}")]
        public async Task<ActionResult<ToolResponseDto>> GetToolByGuidAsync([FromRoute] Guid guid)
        {
            // Get tool
            var tool = await _queryToolsService.GetToolAsync(guid, Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<ToolResponseDto>(tool));
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
        [ProducesResponseType(typeof(PageResultDto<ToolResponseDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<PageResultDto<ToolResponseDto>>> GetToolsAsync([FromQuery] string? searchTerm, [FromQuery] string? name, [FromQuery] ToolType? toolType,
            [FromQuery] string? orderProperty = "rank", [FromQuery] Order order = Order.Descending,
            [FromQuery] uint pageNumber = 1, [FromQuery] uint perPage = 30)
        {
            // Get tools
            var tools = await _queryToolsService.GetToolsAsync(searchTerm, name, toolType, new Page(pageNumber, perPage),
                new SortOrder(orderProperty, order), Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<PageResultDto<ToolResponseDto>>(tools));
        }

        /// <summary>
        /// Create a new tool
        /// </summary>
        /// <param name="model">The tool to create</param>
        /// <returns>The new tool</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ToolResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ToolResponseDto), StatusCodes.Status201Created)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("")]
        public async Task<ActionResult<ToolResponseDto>> CreateToolAsync([FromBody] CreateNewToolRequestDto model)
        {
            // Create tool
            var tool = await _createToolsService.CreateToolAsync(model.Logo, model.Name, model.Description, model.Link, model.Rank,
                model.ToolType, HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<ToolResponseDto>(tool));
        }

        /// <summary>
        /// Update a new tool
        /// </summary>
        /// <param name="model">The tool to update</param>
        /// <returns>The tool</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(ToolResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ToolResponseDto), StatusCodes.Status201Created)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("{toolGuidId}")]
        public async Task<ActionResult<ToolResponseDto>> UpdateToolAsync([FromRoute] Guid toolGuidId, [FromBody] UpdateExistingToolRequestDto model)
        {
            // Update tool
            var updatedTool = await _updateToolsService.UpdateToolAsync(toolGuidId, model.Logo, model.Name, model.Description, model.Link, model.Rank,
                model.ToolType, HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<ToolResponseDto>(updatedTool));
        }
    }
}
