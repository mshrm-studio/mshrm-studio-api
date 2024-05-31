using AspNetCoreRateLimit;
using Hangfire;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.SwaggerUI;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Filters;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Mshrm.Studio.Pricing.Api.Services.Jobs.Interfaces;
using Mshrm.Studio.Pricing.Api.Extensions;
using Mshrm.Studio.Pricing.Api.Context;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSettings();
builder.ConfigureCors();
builder.ConfigureControllers();
builder.ConfigureMVC();
builder.ConfigureWebServer();
builder.ConfigureRateLimiting();
builder.ConfigureCache();
builder.ConfigureHttpClients();
builder.ConfigureOptions();
builder.ConfigureServices();
builder.ConfigureOpenTracing();
builder.ConfigureMediatr();
builder.ConfigureAuthentication();
builder.ConfigureDbContexts();
builder.ConfigureHangfire();
builder.ConfigureAutomapper();
builder.ConfigureSwagger();
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

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(settings =>
{
    settings.PostProcess = document =>
    {
        document.Info.Version = "v1";
        document.Info.Title = "Mshrm.Studio Pricing API";
        document.Info.Description = "REST API";
    };
});

var app = builder.Build();

// Migrate db context
if (builder.Configuration.GetValue<bool>("EFCore:Migrate") == true)
{
    await app.AddDatabaseMigrationAsync<MshrmStudioPricingDbContext>();
}

// Use the request localization
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(builder.Configuration.GetValue<string>("Swagger:EndPoint"), builder.Configuration.GetValue<string>("Swagger:Title"));
        c.RoutePrefix = string.Empty; // Set at root
        c.DocExpansion(DocExpansion.None);
        c.ConfigObject.AdditionalItems.Add("syntaxHighlight", false); //Turns off syntax highlight which causing performance issues...
        c.ConfigObject.AdditionalItems.Add("theme", "agate"); //Reverts Swagger UI 2.x  theme which is simpler not much performance benefit...
    });
}

app.UseDeveloperExceptionPage();

// Global cors policy - handle this in Azure
app.UseCors(x => x.AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

// Turn on rate limiting
app.UseIpRateLimiting();

// Use endpoint routing
app.UseRouting();

app.UseHangfireDashboard();

app.ConfigureHangfireCronJobs();

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCloudEvents();

// Setup endpoints
app.UseEndpoints(endpoints =>
{
    // Only add endpoint if required
    if (builder.Configuration.GetValue<bool>("Endpoints:Enabled"))
    {
        endpoints.MapControllers();
        endpoints.MapSubscribeHandler();

        // Setup hangfire dashboard
        if (app.Environment.IsDevelopment())
        {
            endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
            {
                IgnoreAntiforgeryToken = true,
                Authorization = new[] { new DashboardNoAuthorizationFilter(), }
            });
        }
        else endpoints.MapHangfireDashboard("/hangfire").RequireAuthorization();
    }
});

app.MapHealthChecks("/health");

app.Run();
