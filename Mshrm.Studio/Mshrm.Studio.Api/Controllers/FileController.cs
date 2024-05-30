using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Controllers.Bases;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using Order = Mshrm.Studio.Shared.Enums.Order;

namespace Mshrm.Studio.Api.Controllers
{
    /// <summary>
    /// File API
    /// </summary>
    [ApiController]
    [FormatFilter]
    [Route("api/v1/files")]
    public class FileController : MshrmStudioBaseController
    {
        private readonly IMapper _mapper;
        private readonly IQueryFileService _queryFileService;
        private readonly ICreateFileService _createFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileController"/> class.
        /// </summary>
        /// <param name="domainUserClient"></param>
        /// <param name="queryFileService"></param>
        /// <param name="createFileService"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="mapper"></param>
        public FileController(IDomainUserClient domainUserClient, IQueryFileService queryFileService, ICreateFileService createFileService,
            IHttpContextAccessor contextAccessor, IMapper mapper) : base(domainUserClient, contextAccessor)
        {
            _queryFileService = queryFileService;
            _createFileService = createFileService;

            _mapper = mapper;
        }

        /// <summary>
        /// Get a file
        /// </summary>
        /// <param name="guid">The guid identifier</param>
        /// <returns>A file if exists</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
        [Route("guid/{guid}")]
        public async Task<ActionResult<Stream>> GetPublicFileByGuidAsync([FromRoute] Guid guid)
        {
            // Get stream
            var fileStream = await _queryFileService.StreamPublicFileAsync(guid, Request.HttpContext.RequestAborted);

            return Ok(fileStream);
        }

        /// <summary>
        /// Create a temporary upload
        /// </summary>
        /// <param name="model">The file to upload</param>
        /// <returns>A temp file key</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TemporaryFileUploadResponseDto), StatusCodes.Status200OK)]
        [Route("temporary")]
        public async Task<ActionResult<TemporaryFileUploadResponseDto>> UploadTemporaryFileAsync([FromForm] UploadTemporaryFileDto model)
        {
            // Open read
            using var fileStream = model.File.OpenReadStream();

            var tempFileUploadData = await _createFileService.UploadTemporaryFileAsync(fileStream, model.File.Name, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<TemporaryFileUploadResponseDto>(tempFileUploadData));
        }
    }
}
