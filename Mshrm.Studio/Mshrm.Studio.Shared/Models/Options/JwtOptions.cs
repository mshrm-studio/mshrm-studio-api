using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Models.Options
{
    public class JwtOptions
    {
        public string JwtSigningKey { get; set; }
        public string Audience { get; set; }
        public IEnumerable<string> ValidAudiences { get; set; }
        public string Issuer { get; set; }
        public List<string> ValidIssuers { get; set; } = new List<string>();
        public string RefreshTokenExpiry { get; set; } = "24h";
        public string Expiry { get; set; }
    }
}
