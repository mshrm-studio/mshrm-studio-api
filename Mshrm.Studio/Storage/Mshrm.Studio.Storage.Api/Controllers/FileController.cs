using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Storage.Api.Models.CQRS.Files.Commands;
using Mshrm.Studio.Storage.Api.Models.CQRS.Resources.Queries;
using Mshrm.Studio.Storage.Api.Models.Dtos.Files;
using Mshrm.Studio.Storage.Api.Models.Dtos.Resources;
using Mshrm.Studio.Storage.Api.Models.Entities;
using Mshrm.Studio.Storage.Api.Models.Enums;
using Mshrm.Studio.Storage.Api.Models.Misc;
using System;
using System.IO;

namespace Mshrm.Studio.Storage.Api.Controllers
{
    /// <summary>
    /// File API
    /// </summary>
    [ApiController]
    [Route("api/v1/files")]
    public class FileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FileController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public FileController(IMediator mediator, ILogger<FileController> logger, IMapper mapper)
        {
            _mediator = mediator;

            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Upload a temporary file - this gets deleted in 3 days from creation
        /// </summary>
        /// <param name="model">The file to persist</param>
        /// <returns>TThe temporary file key</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TemporaryFileUploadDto), StatusCodes.Status200OK)]
        [Route("temporary")]
        public async Task<ActionResult<TemporaryFileUploadDto>> UploadTemporaryFileAsync([FromForm] UploadTemporaryFileDto model)
        {
            // Open read
            using var fileStream = model.File.OpenReadStream();

            // Create tempFile
            var tempFileUpload = await _mediator.Send<TemporaryFileUpload>(new UploadTemporaryFileCommand() { Stream = fileStream }, Request.HttpContext.RequestAborted);

            // Upload the new resource and return
            return Ok(_mapper.Map<TemporaryFileUploadDto>(tempFileUpload));
        }

        /// <summary>
        /// Saves a temporary file
        /// </summary>
        /// <param name="model">The file to persist</param>
        /// <returns>The resource saved</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResourceDto), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<ResourceDto>> SaveTemporaryFileAsync([FromBody] SaveTemporaryFileDto model)
        {
            var savedResource = await _mediator.Send<Resource>(_mapper.Map<SaveTemporaryFileCommand>(model), Request.HttpContext.RequestAborted);

            // Upload the new resource and return
            return Ok(_mapper.Map<ResourceDto>(savedResource));
        }

        /// <summary>
        /// Saves temporary file/s
        /// </summary>
        /// <param name="model">The file/s to persist</param>
        /// <returns>The resource/s saved</returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<ResourceDto>), StatusCodes.Status200OK)]
        [Route("multi")]
        public async Task<ActionResult<List<ResourceDto>>> SaveTemporaryFilesAsync([FromBody] SaveTemporaryFilesDto model)
        {
            var savedResources = await _mediator.Send<List<Resource>>(_mapper.Map<SaveTemporaryFilesCommand>(model), Request.HttpContext.RequestAborted);

            // Upload the new resources and return
            return Ok(_mapper.Map<List<ResourceDto>>(savedResources));
        }

        /// <summary>
        /// Get a file
        /// </summary>
        /// <param name="resourceId">The key of the file to get</param>
        /// <param name="fileName">The file name to download as</param>
        /// <returns>The file</returns>
        [HttpGet]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [Route("{resourceId}")]
        public async Task<FileStreamResult> GetFileAsync([FromRoute] Guid resourceId, [FromQuery] string? fileName)
        {
            // Get the file
            var resourceStream = await _mediator.Send<ResourceStream>(new GetResourceStreamQuery() { ResourceId = resourceId }, Request.HttpContext.RequestAborted);

            // Return stream of file
            return File(resourceStream.Stream, resourceStream.ContentType, fileName);
        }
    }
}
