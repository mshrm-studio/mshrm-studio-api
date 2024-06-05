using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Shared.Helpers;
using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;
using Mshrm.Studio.Storage.Api.Models.Enums;
using Mshrm.Studio.Storage.Domain.Files;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mshrm.Studio.Storage.Api.Models.Entities
{
    [Index("GuidId")]
    public class Resource : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public Guid GuidId { get; private set; }
        public AssetType AssetType { get; private set; }
        public string Key { get; private set; }
        public string FileName { get; private set; }
        public string Extension { get; private set; }
        public bool IsPrivate { get; private set; }

        [NotMapped]
        public string ContentType => GetContentType();

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/> class.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileName"></param>
        /// <param name="extension"></param>
        /// <param name="assetType"></param>
        /// <param name="isPrivate"></param>
        public Resource(string key, string fileName, string extension, AssetType assetType, bool isPrivate)
        {
            Key = key;
            FileName = fileName;
            Extension = extension;
            AssetType = assetType;
            IsPrivate = isPrivate;
        }

        /// <summary>
        /// Get the content type from the file name
        /// </summary>
        /// <returns>The files content type</returns>
        public string? GetContentType()
        {
            return new MshrmStudioFile(Key, FileName).GetFileMetaData()?.ContentType;
        }
    }
}
