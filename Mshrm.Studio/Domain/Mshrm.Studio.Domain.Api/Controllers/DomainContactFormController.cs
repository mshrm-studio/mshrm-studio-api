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
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Commands;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Queries;
using Mshrm.Studio.Domain.Api.Models.CQRS.Tools.Queries;
using MediatR;

namespace Mshrm.Studio.Domain.Api.Controllers
{
    /// <summary>
    /// Contact form API
    /// </summary>
    [ApiController]
    [Route("api/v1/contact-forms")]
    public class DomainContactFormController : ControllerBase
    {
        private readonly ILogger<DomainContactFormController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainContactFormController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public DomainContactFormController(IMediator mediator, ILogger<DomainContactFormController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;

            _mediator = mediator;
        }

        /// <summary>
        /// Get a contact form by Guid
        /// </summary>
        /// <param name="guid">The guid identifier</param>
        /// <returns>A contact form if exists</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ContactFormDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("guid/{guid}")]
        public async Task<ActionResult<ContactFormDto>> GetContactFormByGuidAsync([FromRoute] Guid guid)
        {
            // Get contact form
            var contactForm = await _mediator.Send<ContactForm>(new GetContactFormByGuidQuery() { GuidId = guid }, Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<ContactFormDto>(contactForm));
        }

        /// <summary>
        /// Get a page of contact forms
        /// </summary>
        /// <param name="searchTerm">A search term (message)</param>
        /// <param name="userId">The user who created the forms</param>
        /// <param name="createdFrom">The date the contact forms are created from</param>
        /// <param name="createdTo">The date the contact forms are created to</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <returns>A page of contact forms</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResultDto<ContactFormDto>), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("")]
        public async Task<ActionResult<PageResultDto<ContactFormDto>>> GetContactFormsAsync([FromQuery] string? searchTerm, [FromQuery] DateTime? createdFrom,
            [FromQuery] DateTime? createdTo, [FromQuery] string? orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, 
            [FromQuery] uint pageNumber = 1, [FromQuery] uint perPage = 30)
        {
            // Get contact forms
            var contactForms = await _mediator.Send<PagedResult<ContactForm>>(new GetContactFormsPagedQuery()
            {
                SearchTerm = searchTerm,
                CreatedFrom = createdFrom,
                CreatedTo = createdTo,
                Order = order,
                OrderProperty = orderProperty,
                PageNumber = pageNumber,
                PerPage = perPage
            }, Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<PageResultDto<ContactFormDto>>(contactForms));
        }

        /// <summary>
        /// Create a new contact form
        /// </summary>
        /// <param name="dto">The contact form data</param>
        /// <returns>The new contact form</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ContactFormDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ContactFormDto), StatusCodes.Status201Created)]
        [Route("")]
        public async Task<ActionResult<ContactFormDto>> CreateContactFormAsync([FromBody] CreateContactFormDto dto)
        {
            // Create contact form
            var contactForm = await _mediator.Send<ContactForm>(_mapper.Map<CreateContactFormCommand>(dto), Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<ContactFormDto>(contactForm));
        }
    }
}
