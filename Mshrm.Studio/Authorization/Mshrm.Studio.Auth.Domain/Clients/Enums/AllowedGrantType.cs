using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Domain.Clients.Enums
{
    public enum AllowedGrantType
    {
        ClientCredentials = 0,
        Password = 1,
        AuthorizationCode = 2
    }
}
