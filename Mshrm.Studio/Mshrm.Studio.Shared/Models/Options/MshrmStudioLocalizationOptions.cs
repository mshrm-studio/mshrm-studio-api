using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Models.Options
{
    public class MshrmStudioLocalizationOptions
    {
        public List<string> SupportedCultures { get; set; }
        public string DefaultCulture { get; set; }
    }
}
