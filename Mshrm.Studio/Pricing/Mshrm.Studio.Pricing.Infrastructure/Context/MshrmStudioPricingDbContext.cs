using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Mshrm.Studio.Shared.Models.Constants;
using Mshrm.Studio.Shared.Extensions;
using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Maps;

namespace Mshrm.Studio.Pricing.Api.Context
{
    /// <summary>
    /// Domain context
    /// </summary>
    public class MshrmStudioPricingDbContext : DbContext
    {
        /// <summary>
        /// The user performing CRUD
        /// </summary>
        protected int? UserId => this.GetLoggedInUserId();

        /// <summary>
        /// Assets table
        /// </summary>
        public DbSet<Asset> Assets { get; set; }

        /// <summary>
        /// Exchange price pairs table
        /// </summary>
        public DbSet<ExchangePricingPair> ExchangePricingPairs { get; set; }

        /// <summary>
        /// Exchange price pairs history table
        /// </summary>
        public DbSet<ExchangePricingPairHistory> ExchangePricingPairHistories { get; set; }

        /// <summary>
        /// Mediatr
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="options"></param>
        public MshrmStudioPricingDbContext(IMediator mediator, DbContextOptions<MshrmStudioPricingDbContext> options)
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
            _ = new AssetMap(builder.Entity<Asset>());
            _ = new ExchangePricingPairMap(builder.Entity<ExchangePricingPair>());
			_ = new ExchangePricingPairHistoryMap(builder.Entity<ExchangePricingPairHistory>());

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
