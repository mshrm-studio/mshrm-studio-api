using Mshrm.Studio.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Models.Entities.Bases
{
    /// <summary>
    /// Inherit from this is the poco used requires auditing in database
    /// </summary>
    public abstract class AuditableEntity : Entity, IAuditableEntityProperties
    {
        /// <summary>
        /// When the entity was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Who the entity was created by
        /// </summary>
        public int? CreatedById { get; set; }

        /// <summary>
        /// When the entity was updated (if ever)
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Who the entity was updated by (if updated)
        /// </summary>
        public int? UpdatedById { get; set; }
    }
}
