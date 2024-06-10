using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class CreateToolsService : ICreateToolsService
    {
        private readonly IDomainToolsClient _domainToolsClient;
        private readonly IFileClient _fileClient;

        public CreateToolsService(IDomainToolsClient domainToolsClient, IFileClient fileClient)
        {
            _domainToolsClient = domainToolsClient;
            _fileClient = fileClient;
        }

        /// <summary>
        /// Create a new tool
        /// </summary>
        /// <param name="logo">The tools logo</param>
        /// <param name="name">The name</param>
        /// <param name="description">A description</param>
        /// <param name="link">The link to tool</param>
        /// <param name="rank">Display rank</param>
        /// <param name="toolType">The type of tool</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The tool</returns>
        public async Task<ToolDto> CreateToolAsync(TemporaryFileRequestDto logo, string name, string? description, string link, int rank, ToolType toolType, CancellationToken cancellationToken)
        {
            // Create logo
            var persistedLogo = await _fileClient.SaveTemporaryFileAsync(new SaveTemporaryFileDto() { Key = logo.TemporaryKey, FileName = logo.FileName, IsPrivate = false }, cancellationToken);

            // Create a new tool
            return await _domainToolsClient.CreateToolAsync(new CreateToolDto()
            {
                Description = description,
                Link = link,    
                LogoGuidId = persistedLogo.GuidId,
                Name = name,
                Rank = rank,
                ToolType = toolType
            }, cancellationToken);
        }
    }
}
