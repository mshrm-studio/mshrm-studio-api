using Microsoft.AspNetCore.Identity;

namespace Mshrm.Studio.Auth.Api.Models.Entities
{
    public class ResetPasswordReport
    {
        public bool Success { get; set; }
        public List<IdentityError> Errors { get; set; }
    }
}
