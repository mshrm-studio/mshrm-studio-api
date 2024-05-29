namespace Mshrm.Studio.Storage.Api.Models.Misc
{
    public class TemporaryFileUpload
    {
        public string Key { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }
    }
}
