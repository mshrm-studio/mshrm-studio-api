using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Models.Pagination
{
    /// <summary>
    /// Used for pagination requests
    /// </summary>
    public class Page
    {
        /// <summary>
        /// This is the page number of results to get (starts at 0)
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// How many items are returned per page
        /// </summary>
        public int PerPage { get; }

        /// <summary>
        /// Constructor to setup class
        /// </summary>
        public Page(uint pageNumber, uint perPage)
        {
            // Ensure 0 is start for entity framework
            PageNumber = (int)pageNumber;
            PerPage = (int)perPage;
        }
    }
}
