using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Domain.Clients.Enums
{
    public enum AllowedGrantType
    {
        //[EnumMember(Value = "client_credentials")]
        ClientCredentials = 0,
        //[EnumMember(Value = "password")]
        ResourceOwnerPassword = 1,
        //[EnumMember(Value = "authorization_code")]
        AuthorizationCode = 2,
        //[EnumMember(Value = "hybrid")]
        Hybrid = 3,
    }
}
