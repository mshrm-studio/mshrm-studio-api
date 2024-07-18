using Mshrm.Studio.Shared.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Models.Dtos
{
    public class PageResultDto<T>
    {
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// How many items are returned per page (must be >= 0)
        /// </summary>
        [JsonProperty("perPage")]
        public uint PerPage { get; set; }

        /// <summary>
        /// Total number of results in database
        /// </summary>
        [JsonProperty("totalResults")]
        public uint TotalResults { get; set; }

        /// <summary>
        /// The way in which the sorted properties are ordered - default is ascending
        /// </summary>
        [JsonProperty("order")]
        public Order Order { get; set; }

        /// <summary>
        /// The property name to sort by - if null, the requests default is used
        /// </summary>
        [JsonProperty("propertyName")]
        public string? PropertyName { get; set; }

        /// <summary>
        /// The list of results searched for
        /// </summary>
        [JsonProperty("results")]
        public List<T>? Results { get; set; }
    }
}
