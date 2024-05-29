using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Models.Dtos.Files;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IUpdateToolsService
    {
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
        Task<ToolDto> UpdateToolAsync(Guid toolGuidId, TemporaryFileDto? logo, string name, string? description, string link, int rank, 
            ToolType toolType, CancellationToken cancellationToken);
    }
}
