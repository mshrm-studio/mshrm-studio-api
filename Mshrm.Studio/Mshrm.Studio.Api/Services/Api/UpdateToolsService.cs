using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    /// <summary>
    /// For updating tools
    /// </summary>
    public class UpdateToolsService : IUpdateToolsService
    {
        private readonly IDomainToolsClient _domainToolsClient;
        private readonly IFileClient _fileClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateToolsService"/> class.
        /// </summary>
        /// <param name="domainToolsClient"></param>
        /// <param name="fileClient"></param>
        public UpdateToolsService(IDomainToolsClient domainToolsClient, IFileClient fileClient)
        {
            _domainToolsClient = domainToolsClient;
            _fileClient = fileClient;
        }

        /// <summary>
        /// Update an existing tool
        /// </summary>
        /// <param name="toolGuidId">The tools guid ID</param>
        /// <param name="logo">A new logo (or none if not required to be updated)</param>
        /// <param name="name">The new name</param>
        /// <param name="description">A new description</param>
        /// <param name="link">A new link</param>
        /// <param name="rank">The new rank</param>
        /// <param name="toolType">The new tool type</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The updated token</returns>
        public async Task<ToolDto> UpdateToolAsync(Guid toolGuidId, TemporaryFileRequestDto? logo, string name, string? description, string link, int rank,
            ToolType toolType, CancellationToken cancellationToken)
        {
            // Create logo
            ResourceDto? persistedLogo = null;
            if (logo != null)
                persistedLogo = await _fileClient.SaveTemporaryFileAsync(new SaveTemporaryFileDto() { Key = logo.TemporaryKey, FileName = logo.FileName, IsPrivate = false }, cancellationToken);

            // Update the tool
            return await _domainToolsClient.UpdateToolAsync(toolGuidId, new UpdateToolDto()
            {
                Description = description,
                Rank = rank,
                Link = link,
                LogoGuidId = persistedLogo?.GuidId,
                Name = name,
                ToolType = toolType,
            }, cancellationToken);
        }
    }
}
