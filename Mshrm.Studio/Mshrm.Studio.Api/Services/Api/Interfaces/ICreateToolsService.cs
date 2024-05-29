using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Models.Dtos.Files;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface ICreateToolsService
    {
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
        Task<ToolDto> CreateToolAsync(TemporaryFileDto logo, string name, string? description, string link, int rank, ToolType toolType, CancellationToken cancellationToken);
    }
}
