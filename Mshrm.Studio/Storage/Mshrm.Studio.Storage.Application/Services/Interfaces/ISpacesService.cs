namespace Mshrm.Studio.Storage.Api.Services.Http.Interfaces
{
    public interface ISpacesService
    {
        /// <summary>
        /// Upload a file to S3 bucket
        /// </summary>
        /// <param name="file">The file stream</param>
        /// <param name="fileName">The name to save file as</param>
        /// <param name="filepath">The folder path the file will live at</param>
        /// <returns>Saved filename if uploaded</returns>
        public Task<string?> UploadFileAsync(Stream file, string fileName, string? filepath = null);

        /// <summary>
        /// Delette a file from S3 bucket
        /// </summary>
        /// <param name="key">The file to delete</param>
        /// <param name="filepath">The folder path the file will live at</param>
        /// <returns>Saved filename</returns>
        public Task<bool> DeleteFileAsync(string key, string? filepath = "temp");

        /// <summary>
        /// Get a file from S3
        /// </summary>
        /// <param name="key">The file to get</param>
        /// <param name="filePath">A sub-folder path to get from</param>
        /// <returns>A file from S3 if exists</returns>
        public Task<Stream?> GetFileAsync(string key, string? filePath = null);

        /// <summary>
        /// Move a file from temporary folder into permanent folder
        /// </summary>
        /// <param name="key">The temp key</param>
        /// <param name="filePath">A sub-folder path to get from</param>
        /// <returns>The files final location key</returns>
        Task<string?> MoveTemporaryFileAsync(string key, string? filePath = null);
    }
}
