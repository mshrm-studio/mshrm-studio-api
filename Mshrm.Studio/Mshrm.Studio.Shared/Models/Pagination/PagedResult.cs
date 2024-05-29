using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Models.Pagination
{
    public class PagedResult<T>
    {
        public Page Page { get; set; }
        public SortOrder SortOrder { get; set; }
        public int TotalResults { get; set; }
        public List<T> Results { get; set; }
    }
}
