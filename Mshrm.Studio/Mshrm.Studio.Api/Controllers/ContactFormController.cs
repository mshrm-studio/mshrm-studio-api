using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Controllers.Bases;
using Mshrm.Studio.Api.Models.Dtos.ContactForm;
using Mshrm.Studio.Api.Models.Dtos.ContactForms;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using System.Collections.Generic;
using Order = Mshrm.Studio.Shared.Enums.Order;

namespace Mshrm.Studio.Api.Controllers
{
    /// <summary>
    /// Contact form API
    /// </summary>
    [ApiController]
    [FormatFilter]
    [Route("api/v1/contact-forms")]
    public class ContactFormController : MshrmStudioBaseController
    {
        private readonly IQueryContactFormService _queryContactFormService;
        private readonly ICreateContactFormService _createContactFormService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactFormController"/> class.
        /// </summary>
        /// <param name="queryContactFormService"></param>
        /// <param name="createContactFormService"></param>
        /// <param name="domainUserClient"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="mapper"></param>
        public ContactFormController(IQueryContactFormService queryContactFormService, ICreateContactFormService createContactFormService, IDomainUserClient domainUserClient,
            IHttpContextAccessor contextAccessor, IMapper mapper) : base(domainUserClient, contextAccessor)
        {
            _queryContactFormService = queryContactFormService;
            _createContactFormService = createContactFormService;

            _mapper = mapper;
        }

        /// <summary>
        /// Get a contact form by Guid
        /// </summary>
        /// <param name="guid">The guid identifier</param>
        /// <returns>A contact form if exists</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ContactFormResponseDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("guid/{guid}")]
        public async Task<ActionResult<ContactFormResponseDto>> GetContactFormByGuidAsync([FromRoute] Guid guid)
        {
            // Get contact form
            var contactForm = await _queryContactFormService.GetContactFormAsync(guid, await GetUserAsync(), GetUserRole(), Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<ContactFormResponseDto>(contactForm));
        }

        /// <summary>
        /// Get a page of contact forms
        /// </summary>
        /// <param name="searchTerm">A search term (message)</param>
        /// <param name="contactEmail">The contact email</param>
        /// <param name="createdFrom">The date the contact forms are created from</param>
        /// <param name="createdTo">The date the contact forms are created to</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <returns>A page of contact forms</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResultDto<ContactFormResponseDto>), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
        [Route("")]
        public async Task<ActionResult<PageResultDto<ContactFormResponseDto>>> GetContactFormsAsync([FromQuery] string? searchTerm, [FromQuery] string? contactEmail, [FromQuery] DateTime? createdFrom,
            [FromQuery] DateTime? createdTo, [FromQuery] string? orderProperty = "createdDate", [FromQuery] Order order = Order.Descending,
            [FromQuery] uint pageNumber = 1, [FromQuery] uint perPage = 30)
        {
            // Get contact forms
            var contactForms = await _queryContactFormService.GetContactFormsAsync(searchTerm, contactEmail, createdFrom, createdTo, await GetUserAsync(), GetUserRole(),
                new Page(pageNumber, perPage), new SortOrder(orderProperty, order), Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<PageResultDto<ContactFormResponseDto>>(contactForms));
        }

        /// <summary>
        /// Create a new contact form
        /// </summary>
        /// <param name="dto">The contact form data</param>
        /// <returns>The new contact form</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ContactFormResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ContactFormResponseDto), StatusCodes.Status201Created)]
        [Route("")]
        public async Task<ActionResult<ContactFormResponseDto>> CreateContactFormAsync([FromBody] CreateNewContactFormRequestDto dto)
        {
            // Create contact form
            var contactForm = await _createContactFormService.CreateContactFormAsync(dto.Message, dto.ContactEmail, dto.FirstName, dto.LastName, dto.WebsiteUrl,
                dto.TemporaryAttachmentKeys, HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<ContactFormResponseDto>(contactForm));
        }
    }
}
