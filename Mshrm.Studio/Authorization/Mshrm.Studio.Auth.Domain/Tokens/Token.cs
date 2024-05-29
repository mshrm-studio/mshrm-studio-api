
namespace Mshrm.Studio.Auth.Api.Models.Entities
{
    public class Token
    {
        public string TokenValue { get; set; }
        public string RefreshTokenValue { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
