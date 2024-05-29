using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Enums
{
    /// <summary>
    /// This defines audit property names for poco database models
    /// </summary>
    public enum AuditProperty
    {
        /// <summary>
        /// The created date of the entity
        /// </summary>
        CreatedDate = 0,

        /// <summary>
        /// Who the entity was created by
        /// </summary>
        CreatedById = 1,

        /// <summary>
        /// The date of the entity update
        /// </summary>
        UpdatedDate = 2,

        /// <summary>
        /// Who the entity was updated by
        /// </summary>
        UpdatedById = 4
    }
}
