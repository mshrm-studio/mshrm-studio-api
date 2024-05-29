using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Interfaces;
using Mshrm.Studio.Shared.Models.Entities;
using Mshrm.Studio.Shared.Models.Entities.NewFolder;

namespace Mshrm.Studio.Shared.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Add audit to entity being persisted (if accepts audit)
        /// </summary>
        /// <param name="context">The db context to listen to (on state change)</param>
        /// <param name="userId">The user updating entity</param>
        public static void AddAuditProperties(this DbContext context, int? userId)
        {
            foreach (EntityEntry<IAuditableEntityProperties> entry in context.ChangeTracker.Entries<IAuditableEntityProperties>())
            {
                // Update the inserted audit values
                if (entry.State == EntityState.Added)
                {
                    entry.Property(AuditProperty.CreatedById.ToString()).CurrentValue = string.IsNullOrEmpty((string)entry.Property(AuditProperty.CreatedById.ToString()).CurrentValue)
                        ? userId : entry.Property(AuditProperty.CreatedById.ToString()).CurrentValue;
                    entry.Property(AuditProperty.CreatedDate.ToString()).CurrentValue = (entry.Property(AuditProperty.CreatedDate.ToString()).CurrentValue != null) ? DateTime.UtcNow :
                        entry.Property(AuditProperty.CreatedDate.ToString()).CurrentValue;
                }
                // Update the updated audit values
                else if (entry.State == EntityState.Modified) // Existing entity updated
                {
                    entry.Property(AuditProperty.UpdatedById.ToString()).CurrentValue = userId;
                    entry.Property(AuditProperty.UpdatedDate.ToString()).CurrentValue = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Dispatch any events
        /// </summary>
        /// <param name="context">The context to try to dispatch events for</param>
        /// <param name="mediator">The place to dispatch events to</param>
        /// <returns>An async task</returns>
        public static async Task DispatchEvents(this DbContext context, IMediator mediator)
        {
            var domainEventEntities = context.ChangeTracker.Entries<IDomainEvent>()
                .Select(po => po.Entity)
                .Where(po => po.DomainEvents.Any())
                .ToArray();

            foreach (var entity in domainEventEntities)
            {
                var events = entity.DomainEvents.ToArray();
                entity.DomainEvents.Clear();
                foreach (var entityDomainEvent in events)
                    await mediator.Publish(entityDomainEvent);
            }
        }
    }
}
