using Mshrm.Studio.Storage.Api.Models.Entities;
using Mshrm.Studio.Storage.Api.Models.Enums;

namespace Mshrm.Studio.Storage.Api.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        /// <summary>
        /// Create a new resource
        /// </summary>
        /// <param name="key">The s3 key</param>
        /// <param name="fileName">The file name</param>
        /// <param name="extension">The extension of the file</param>
        /// <param name="assetType">The type of file</param>
        /// <param name="isPrivate">If resource is only accessable with auth</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The new resource</returns>
        public Task<Resource> CreateResourceAsync(string key, string fileName, string extension, AssetType assetType, bool isPrivate, CancellationToken cancellationToken);

        /// <summary>
        /// Get a resource
        /// </summary>
        /// <param name="resourceId">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A resource if exists</returns>
        public Task<Resource?> GetResourceAsync(Guid resourceId, CancellationToken cancellationToken);
    }
}
