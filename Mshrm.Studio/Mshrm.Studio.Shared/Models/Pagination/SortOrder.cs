using Hangfire.Server;
using Mshrm.Studio.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Models.Pagination
{
    /// <summary>
    /// Used to map sort order (property/order)
    /// </summary>
    public class SortOrder
    {
        /// <summary>
        /// The way in which the sorted properties are ordered
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// The property name to sort by
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Constructor to setup class
        /// </summary>
        public SortOrder(string propertyName, Order order)
        {
            PropertyName = propertyName;
            Order = order;
        }
    }
}
