using Microsoft.AspNetCore.Identity;
using Mshrm.Studio.Shared.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mshrm.Studio.Auth.Domain.Roles
{
    public class MshrmStudioIdentityUserRole : IdentityUserRole<string>
    {
        [NotMapped]
        public string Email { get; set; }

        [NotMapped]
        public string Role { get; set; }

        public void SetupForDefaultImport()
        {
            RoleId = Role.GenerateSeededGuid().ToString();
            UserId = Email.GenerateSeededGuid().ToString();
        }
    }
}
