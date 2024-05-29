using Mshrm.Studio.Storage.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Storage.Domain.Files
{
    public class MshrmStudioFile
    {
        public string FileName { get; private set; }

        public MshrmStudioFile(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Determines asset type from file extension
        /// </summary>
        /// <param name="fileName">The file name to gleam the extension from</param>
        /// <returns>The correct asset type</returns>
        public FileMetaData GetFileMetaData()
        {
            string contentType;
            AssetType assetType;

            // Find index used to gleam file extension
            var index = FileName.LastIndexOf(".");
            if (index <= 0)
                throw new Exception("Asset type could not be determined as file name has no extension");

            // Get the extension
            var extension = FileName.Substring(index + 1)?.ToLower()?.Trim();

            // Get the asset type + content type
            switch (extension)
            {
                case "jpg":
                case "png":
                case "jpeg":
                case "gif":
                case "tiff":
                case "psd":
                case "raw":
                case "bmp":
                case "heif":
                case "indo":
                    assetType = AssetType.Image;
                    contentType = $"image/{extension}";
                    break;
                case "mp4":
                case "m4a":
                case "m4v":
                case "f4v":
                case "f4a":
                case "m4b":
                case "m4r":
                case "f4b":
                case "mov":
                case "ogg":
                case "oga":
                case "ogv":
                case "ogx":
                case "wmv":
                case "wma":
                case "flv":
                case "avi":
                    assetType = AssetType.Video;
                    contentType = $"video/{extension}";
                    break;
                default:
                    assetType = AssetType.Document;
                    contentType = $"application/{extension}";
                    break;
            };

            return new FileMetaData()
            {
                ContentType = contentType,
                AssetType = assetType,
                Extension = extension,
                FileName = FileName,
            };
        }
    }
}
