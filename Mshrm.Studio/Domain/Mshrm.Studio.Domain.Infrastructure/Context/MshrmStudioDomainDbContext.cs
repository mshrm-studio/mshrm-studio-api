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
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.EntityMaps;
using Mshrm.Studio.Shared.Extensions;
using MediatR;
using Mshrm.Studio.Shared.Helpers;
using Mshrm.Studio.Shared.Models.Constants;

namespace Mshrm.Studio.Domain.Api.Context
{
    /// <summary>
    /// Domain context
    /// </summary>
    public class MshrmStudioDomainDbContext : DbContext
    {
        /// <summary>
        /// The user performing CRUD
        /// </summary>
        protected int? UserId => this.GetLoggedInUserId();

        /// <summary>
        /// User table
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Contact form table
        /// </summary>
        public DbSet<ContactForm> ContactForms { get; set; }

        /// <summary>
        /// Tools table
        /// </summary>
        public DbSet<Tool> Tools { get; set; }

        /// <summary>
        /// Mediatr
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="options"></param>
        public MshrmStudioDomainDbContext(DbContextOptions<MshrmStudioDomainDbContext> options, IMediator mediator)
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
            _ = new UserMap(builder.Entity<User>());
            _ = new ContactFormMap(builder.Entity<ContactForm>());
            _ = new ToolMap(builder.Entity<Tool>());

            base.OnModelCreating(builder);

            SetupData(builder);
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

        private void SetupData(ModelBuilder builder)
        {
            var defaultUsers = JsonConvertHelper.FromFile<List<User>>("Resources/defaultUsers.json")
                .Select((x,y) => { x.SetIds(y + 1); return x; })
                .ToList();

            builder.Entity<User>().HasData(defaultUsers);
        }

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
