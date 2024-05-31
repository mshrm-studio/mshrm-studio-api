using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon;
using Mshrm.Studio.Storage.Api.Models.Options;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Storage.Api.Services.Http.Interfaces;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace Mshrm.Studio.Storage.Api.Services.Http
{
    /// <summary>
    /// DDigital Ocean S3 Spaces service
    /// </summary>
    public class SpacesService : ISpacesService
    {
        private readonly DigitalOceanSpacesOptions _options;
        private readonly ILogger<SpacesService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpacesService"/> class.
        /// </summary>
        /// <param name="logger">The class logger</param>
        /// <param name="options">The class options</param>
        public SpacesService(ILogger<SpacesService> logger, IOptions<DigitalOceanSpacesOptions> options)
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Upload a file to S3 bucket
        /// </summary>
        /// <param name="file">The file stream</param>
        /// <param name="key">The name to save file as</param>
        /// <param name="filePath">The folder path the file will live at</param>
        /// <returns>Saved filename</returns>
        public async Task<string?> UploadFileAsync(Stream file, string key, string? filePath = null)
        {
            // Build S3 client
            var client = CreateClient();

            // Init util to send request
            var fileTransferUtility = new TransferUtility(client);

            // Setup request
            var fileTransferRequest = BuildTransferRequest(file, key, filePath);

            // Send request
            await fileTransferUtility.UploadAsync(fileTransferRequest);

            return key;
        }

        /// <summary>
        /// Delete a file from S3 bucket
        /// </summary>
        /// <param name="key">The file to delete</param>
        /// <param name="filePath">The folder path the file will live at</param>
        /// <returns>Saved filename</returns>
        public async Task<bool> DeleteFileAsync(string key, string? filePath = "temp")
        {
            // Build S3 client
            var client = CreateClient();

            // Setup request
            var deleteRequest = BuildDeleteRequest(key, filePath);

            // Send request
            var response = await client.DeleteObjectAsync(deleteRequest);

            return true;
        }

        /// <summary>
        /// Get a file from S3
        /// </summary>
        /// <param name="key">The file to get</param>
        /// <param name="filePath">A sub-folder path to get from</param>
        /// <returns>A file from S3 if exists</returns>
        public async Task<Stream?> GetFileAsync(string key, string? filePath = null)
        {
            // Build S3 client
            var client = CreateClient();

            // Init util to send request
            var fileTransferUtility = new TransferUtility(client);

            // Build full name
            var keyWithSubFolder = (!string.IsNullOrEmpty(filePath) ? (filePath + @"/") : string.Empty) + key;

            // Send request
            return await fileTransferUtility.OpenStreamAsync(_options.Bucket, keyWithSubFolder);
        }

        /// <summary>
        /// Move a file from temporary folder into permanent folder
        /// </summary>
        /// <param name="key">The temp key</param>
        /// <param name="filePath">A sub-folder path to get from</param>
        /// <returns>The files final location key</returns>
        public async Task<string?> MoveTemporaryFileAsync(string key, string? filePath = null)
        {
            // Add the temp part onto key
            var tempKey = $"temp/{key}";

            // Get old one
            using var existingFile = await GetFileAsync(tempKey);
            if (existingFile == null)
                return null;

            // Create new one
            var newFileKey = await UploadFileAsync(existingFile, filePath);
            if (!string.IsNullOrEmpty(newFileKey))
                await DeleteFileAsync(key);

            return newFileKey;
        }

        #region Helpers

        /// <summary>
        /// Create an S3 client
        /// </summary>
        /// <returns>S3 Client</returns>
        public AmazonS3Client CreateClient()
        {
            return new AmazonS3Client(_options.Key, _options.Secret, new AmazonS3Config()
            {
                ServiceURL = $"https://{_options.Region}.digitaloceanspaces.com",
            });
        }

        /// <summary>
        /// Build a transfer request to s3
        /// </summary>
        /// <param name="file">The file stream</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="filePath">The S3 folder path</param>
        /// <returns>Upload request</returns>
        public TransferUtilityUploadRequest BuildTransferRequest(Stream file, string fileName, string? filePath = null)
        {
            var request = new TransferUtilityUploadRequest
            {
                BucketName = _options.Bucket,
                InputStream = file,
                StorageClass = S3StorageClass.Standard,
                Key = (!string.IsNullOrEmpty(filePath) ? (filePath + @"/") : string.Empty) + fileName, // filepath is your folder name in digital ocean space leave empty if not any.
                //CannedACL = CalculateVisibility(filePath) //S3CannedACL.PublicRead TODO:
            };

            var newHeaders = new Dictionary<string, string>()
            {
                { "Content-Type", "application/octet-stream; charset=utf-8" },
                { "Cache-Control","no-cache" }
            };

            foreach (KeyValuePair<string, string> header in newHeaders)
            {
                request.Headers[header.Key] = header.Value;
            }

            return request;
        }

        /// <summary>
        /// Build a delete request to s3
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="filePath">The S3 folder path</param>
        /// <returns>Delete request</returns>
        public DeleteObjectRequest BuildDeleteRequest(string fileName, string? filePath = null)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _options.Bucket,
                Key = (!string.IsNullOrEmpty(filePath) ? (filePath + @"/") : string.Empty) + fileName, // filePath is the folder name in digital ocean space leave empty if not any.
            };

            return request;
        }

        #endregion
    }
}
