using AspNetCoreRateLimit;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Auth.Api.Context;
using Mshrm.Studio.Auth.Web.Extensions;
using Mshrm.Studio.Auth.Web.Middleware;
using Mshrm.Studio.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.ConfigureSettings();
builder.ConfigureCors();
builder.ConfigureControllers();
builder.ConfigureWebServer();
builder.ConfigureRateLimiting();
builder.ConfigureHttpClients();
builder.ConfigureOptions();
builder.ConfigureServices();
builder.ConfigureMediatr();
builder.ConfigureOpenTracing();
builder.ConfigureIdentity();
builder.ConfigureDbContexts();
builder.ConfigureAuthentication();
builder.ConfigureHostedServices();
builder.ConfigureLocalization();
builder.ConfigureHellang();
builder.ConfigureDapr();
builder.ConfigureHealthChecks();

//builder.Services.LoadErrorMappings();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddResponseCompression();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging();

var app = builder.Build();

// Use the request localization
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

// Migrate db context
if (builder.Configuration.GetValue<bool>("EFCore:Migrate") == true)
{
    await app.AddDatabaseMigrationAsync<MshrmStudioAuthDbContext>();

    await app.AddDatabaseMigrationAsync<PersistedGrantDbContext>();
    await app.AddDatabaseMigrationAsync<ConfigurationDbContext>();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

if (!builder.Environment.IsDevelopment())
{
    app.UseMiddleware<IdentityOriginSettingMiddleware>();

    app.Use((context, next) =>
    {
        context.Request.Scheme = "https";
        return next(context);
    });

}

// Turn on rate limiting
app.UseIpRateLimiting();

// Use endpoint routing
app.UseRouting();

app.UseIdentityServer();

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCloudEvents();

app.MapRazorPages();

app.Run();
