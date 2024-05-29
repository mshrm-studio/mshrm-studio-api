using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Interfaces
{
    /// <summary>
    /// Contract for an auditable entity
    /// </summary>
    public interface IAuditableEntityProperties
    {
        /// <summary>
        /// The date an entity was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// The user creating an entity
        /// </summary>
        public int? CreatedById { get; set; }

        /// <summary>
        /// The updated date of an entity
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Who updated an entity
        /// </summary>
        public int? UpdatedById { get; set; }
    }
}
