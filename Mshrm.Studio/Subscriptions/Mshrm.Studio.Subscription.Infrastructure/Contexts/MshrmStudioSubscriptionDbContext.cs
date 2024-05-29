using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mshrm.Studio.Shared.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Http;
using Mshrm.Studio.Shared.Models.Constants;
using Mshrm.Studio.Subscription.Infrastructure.Mappers;
using Mshrm.Studio.Subscription.Domain.Subscriptions;
using Mshrm.Studio.Subscription.Domain.PricingStructures;

namespace Mshrm.Studio.Subscription.Infrastructure.Contexts
{
    /// <summary>
    /// Domain context
    /// </summary>
    public class MshrmStudioSubscriptionDbContext : DbContext
    {
        /// <summary>
        /// The user performing CRUD
        /// </summary>
        protected int? UserId => this.GetLoggedInUserId();

        /// <summary>
        /// Subscription table
        /// </summary>
        public DbSet<SubscriptionModel> SubscriptionModels { get; set; }

        /// <summary>
        /// Mediatr
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="options"></param>
        public MshrmStudioSubscriptionDbContext(DbContextOptions<MshrmStudioSubscriptionDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Called on model creation (mapping can be done here)
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Map entities
            _ = new SubscriptionModelMap(builder.Entity<SubscriptionModel>());
            _ = new PricingStructureMap(builder.Entity<PricingStructure>());

            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Save changes to the context asynchronously
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for operation shutdown</param>
        /// <returns>Number of roes updated</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Populate the audit information
            this.AddAuditProperties(UserId);

            // Save
            var savedCount = await base.SaveChangesAsync(cancellationToken);

            // Dispatch events
            await this.DispatchEvents(_mediator);

            return savedCount;
        }

        /// <summary>
        /// Clear the change tracker
        /// </summary>
        public void ClearChangeTracker()
        {
            ChangeTracker.Clear();
        }

        #region Helpers

        /// <summary>
        /// Get a logged in userId from JWT
        /// </summary>
        /// <returns>A corresponding user ID</returns>
        private int? GetLoggedInUserId()
        {
            // Get username from claims
            var rawUserId = this.GetService<IHttpContextAccessor>().HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimNames.UserId)?.Value;

            // Parse it and return if resolved
            var parsed = int.TryParse(rawUserId, out int userId);
            if (parsed)
                return userId;

            // Otherwise return null
            return null;
        }

        #endregion
    }
}
