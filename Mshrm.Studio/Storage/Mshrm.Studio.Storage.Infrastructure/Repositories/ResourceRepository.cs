using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Shared.Repositories.Bases;
using Mshrm.Studio.Storage.Api.Contexts;
using Mshrm.Studio.Storage.Api.Models.Entities;
using Mshrm.Studio.Storage.Api.Models.Enums;
using Mshrm.Studio.Storage.Api.Repositories.Interfaces;
using Mshrm.Studio.Storage.Domain.Files;
using Mshrm.Studio.Storage.Domain.Resources;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Storage.Api.Repositories
{
    /// <summary>
    /// Repository for resources
    /// </summary>
    public class ResourceRepository : BaseRepository<Resource, MshrmStudioStorageDbContext>, IResourceRepository
    {
        private readonly IResourceFactory _resourceFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ResourceRepository(MshrmStudioStorageDbContext context, IResourceFactory resourceFactory) : base(context)
        {
            _resourceFactory = resourceFactory;
        }

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
        public async Task<Resource> CreateResourceAsync(string key, string fileName, string extension, AssetType assetType, bool isPrivate, CancellationToken cancellationToken)
        {
            // Build new resource to save
            var resource = _resourceFactory.CreateResource(key, fileName, extension, assetType, isPrivate);

            Add(resource);
            await SaveAsync(cancellationToken);

            return resource;
        }

        /// <summary>
        /// Create new resources
        /// </summary>
        /// <param name="files">Files to add resources for</param>
        /// <param name="isPrivate">If resource is only accessable with auth</param>
        /// <param name="cancellationToken">Stopping token</param>
        /// <returns>Resources added</returns>
        public async Task<List<Resource>> CreateResourcesAsync(List<MshrmStudioFile> files, bool isPrivate, CancellationToken cancellationToken)
        {
            var resources = new List<Resource>();

            foreach (var file in files)
            {
                var fileMetaData = file.GetFileMetaData();

                // Build new resource to save
                var resource = _resourceFactory.CreateResource(file.Key, file.FileName, fileMetaData.Extension, fileMetaData.AssetType, isPrivate);

                resources.Add(resource);    
            }

            AddRange(resources);
            await SaveAsync(cancellationToken);

            return resources;
        }

        /// <summary>
        /// Get a resource
        /// </summary>
        /// <param name="resourceId">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A resource if exists</returns>
        public async Task<Resource?> GetResourceAsync(Guid resourceId, CancellationToken cancellationToken)
        {
            return await GetAll()
                .Where(x => x.GuidId == resourceId)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
