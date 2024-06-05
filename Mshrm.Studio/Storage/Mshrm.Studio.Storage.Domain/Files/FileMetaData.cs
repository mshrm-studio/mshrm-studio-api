using Mshrm.Studio.Storage.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Storage.Domain.Files
{
    public class FileMetaData
    {
        public AssetType AssetType { get; set; }
        public string Key { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
    }
}
