using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Models.Enums;
using Mshrm.Studio.Localization.Api.Models.Maps;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Helpers;
using Mshrm.Studio.Shared.Models.Constants;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace Mshrm.Studio.Localization.Api.Contexts
{
    public class MshrmStudioLocalizationDbContext : DbContext
    {
        /// <summary>
        /// The user performing CRUD
        /// </summary>
        protected int? UserId => this.GetLoggedInUserId();

        /// <summary>
        /// Localization table
        /// </summary>
        public DbSet<LocalizationResource> LocalizationResources { get; set; }


        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="options"></param>
        public MshrmStudioLocalizationDbContext(IMediator mediator, DbContextOptions<MshrmStudioLocalizationDbContext> options)
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
            _ = new LocalizationResourceMap(builder.Entity<LocalizationResource>());

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
            var defaultLocalizationResources = JsonConvertHelper.FromFile<List<LocalizationResource>>("Resources/defaultResources.json")
                .Select((x,y) => {
                    x.SetupForDefaultImport(y + 1,"en-US"); 
                    return x;
                }).ToList(); 
           
            builder.Entity<LocalizationResource>().HasData(defaultLocalizationResources);
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
