using AutoMapper;
using Dapr;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Email.Api.Models.CQRS.Emails.Commands;
using Mshrm.Studio.Email.Api.Models.Dtos;
using Mshrm.Studio.Email.Api.Models.Entities;
using Mshrm.Studio.Email.Api.Services.Api;

namespace Mshrm.Studio.Email.Api.Controllers
{
    /// <summary>
    /// Email API
    /// </summary>
    [ApiController]
    [Route("api/v1/email")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public EmailController(IMediator mediator, ILogger<EmailController> logger, IMapper mapper)
        {
            _mediator = mediator;

            _logger = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Send an email address
        /// </summary>
        /// <param name="model">The email to send</param>
        /// <returns>The sent email</returns>
        [HttpPost]
        [ProducesResponseType(typeof(EmailMessageDto), StatusCodes.Status200OK)]
        [Topic("pubsub", "send-email")]
        [Route("")]
        public async Task<ActionResult<EmailMessageDto>> SendSingleEmailAsync([FromBody] EmailDto model)
        {
            // Send message
            var message = await _mediator.Send<EmailMessage>(_mapper.Map<CreateEmailCommand>(model), Request.HttpContext.RequestAborted);

            // Return the message sent
            return Ok(_mapper.Map<EmailMessageDto>(message));
        }
    }
}
