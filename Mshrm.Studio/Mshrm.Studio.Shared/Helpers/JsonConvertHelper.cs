using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Helpers
{
    public static class JsonConvertHelper
    {
        public static T FromFile<T>(string fileName)
        {
            var import = File.ReadAllText(fileName);

            return JsonConvert.DeserializeObject<T>(import);
        }
    }
}
