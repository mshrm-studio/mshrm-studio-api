using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Globalization;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Shared.Extensions;
using MediatR;
using Mshrm.Studio.Shared.Helpers;
using Mshrm.Studio.Auth.Domain.Roles;

namespace Mshrm.Studio.Auth.Api.Context
{
    /// <summary>
    /// Login context
    /// </summary>
    public class MshrmStudioAuthDbContext : IdentityDbContext<MshrmStudioIdentityUser, IdentityRole, string>
    {
        /// <summary>
        /// Mediatr
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="options">Db startup options</param>
        public MshrmStudioAuthDbContext(IMediator mediator, DbContextOptions<MshrmStudioAuthDbContext> options)
            : base(options)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Called when the model is creating - execute database-model mapping here
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
      
            // Add roles 
            SetupData(builder);
        }

        /// <summary>
        /// Save changes to the context asynchronously
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for operation shutdown</param>
        /// <returns>Number of roes updated</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Save
            var savedCount = await base.SaveChangesAsync(cancellationToken);

            // Dispatch events
            await this.DispatchEvents(_mediator);

            return savedCount;
        }

        #region Helpers

        private void SetupData(ModelBuilder builder)
        {
            // Roles
            builder.Entity<IdentityRole>().HasData(
               new IdentityRole { Id = "Admin".GenerateSeededGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
               new IdentityRole { Id = "User".GenerateSeededGuid().ToString(), Name = "User", NormalizedName = "USER" }
            );

            // Users
            var defaultUsers = JsonConvertHelper.FromFile<List<MshrmStudioIdentityUser>>("Resources/defaultUsers.json")
                .Select((x, y) => {
                    x.SetRandomGuidId();
                    return x;
                }).ToList();

            builder.Entity<MshrmStudioIdentityUser>().HasData(defaultUsers);

            // Users roles
            var defaultUsersRoles = JsonConvertHelper.FromFile<List<MshrmStudioIdentityUserRole>>("Resources/defaultUserRoles.json")
                .Select((x, y) => {
                    x.SetupForDefaultImport();
                    return x;
                })
                .Select(x => new IdentityUserRole<string>()
                {
                    UserId = x.UserId,
                    RoleId = x.RoleId,  
                })
                .ToList();

            builder.Entity<IdentityUserRole<string>>().HasData(defaultUsersRoles);
        }

        #endregion
    }
}
