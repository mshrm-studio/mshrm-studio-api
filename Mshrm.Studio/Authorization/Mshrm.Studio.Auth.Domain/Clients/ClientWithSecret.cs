using Duende.IdentityServer.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Domain.Clients
{
    public class ClientWithSecret
    {
        public Client Client { get; set; }
        public string PlainTextSecret { get; set; }
    }
}
