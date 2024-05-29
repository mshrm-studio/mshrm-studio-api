using Mshrm.Studio.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Models.Dtos
{
    public class PageResultDto<T>
    {
        /// <summary>
        /// This is the page number of results to get - starts at 0 (must be >= 0)
        /// </summary>
        public uint PageNumber { get; set; }

        /// <summary>
        /// How many items are returned per page (must be >= 0)
        /// </summary>
        public uint PerPage { get; set; }

        /// <summary>
        /// Total number of results in database
        /// </summary>
        public uint TotalResults { get; set; }

        /// <summary>
        /// The way in which the sorted properties are ordered - default is ascending
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// The property name to sort by - if null, the requests default is used
        /// </summary>
        public string? PropertyName { get; set; }

        /// <summary>
        /// The list of results searched for
        /// </summary>
        public List<T>? Results { get; set; }
    }
}
