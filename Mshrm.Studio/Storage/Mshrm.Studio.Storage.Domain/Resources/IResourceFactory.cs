using Mshrm.Studio.Storage.Api.Models.Entities;
using Mshrm.Studio.Storage.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Storage.Domain.Resources
{
    public interface IResourceFactory
    {
        /// <summary>
        /// Create a new resource
        /// </summary>
        /// <param name="key">The resource key</param>
        /// <param name="fileName">The name of resource</param>
        /// <param name="extension">The extension</param>
        /// <param name="assetType">The type of resource</param>
        /// <param name="isPrivate">If is private (auth access)</param>
        /// <returns>A resource</returns>
        public Resource CreateResource(string key, string fileName, string extension, AssetType assetType, bool isPrivate);
    }
}
