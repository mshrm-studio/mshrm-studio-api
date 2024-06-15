
using AspNetCoreRateLimit;
using Hangfire;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Api.Controllers;
using Mshrm.Studio.Api.Extensions;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSettings();
builder.ConfigureCors();
builder.ConfigureLocalization();
builder.ConfigureControllers();
builder.ConfigureMVC();
builder.ConfigureWebServer();
builder.ConfigureRateLimiting();
builder.ConfigureHttpClients();
builder.ConfigureOptions();
builder.ConfigureServices();
builder.ConfigureOpenTracing();
builder.ConfigureAuthentication();
builder.ConfigureAutomapper();
builder.ConfigureSwagger();
builder.ConfigureHostedServices();
builder.ConfigureHellang();
builder.ConfigureHealthChecks();

//builder.Services.LoadErrorMappings();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddResponseCompression();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(builder.Configuration.GetValue<string>("Swagger:EndPoint"), builder.Configuration.GetValue<string>("Swagger:Title"));
    c.RoutePrefix = string.Empty; // Set at root
    c.DocExpansion(DocExpansion.None);
    c.ConfigObject.AdditionalItems.Add("syntaxHighlight", false); //Turns off syntax highlight which causing performance issues...
    c.ConfigObject.AdditionalItems.Add("theme", "agate"); //Reverts Swagger UI 2.x  theme which is simpler not much performance benefit...
});
//}

app.UseExceptionHandler();

// Use the request localization
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

// Global cors policy - handle this in Azure
app.UseCors(x => x.AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

// Turn on rate limiting
app.UseIpRateLimiting();

// Use endpoint routing
app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next(context);
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Setup endpoints
app.UseEndpoints(endpoints =>
{
    // Only add endpoint if required
    if (builder.Configuration.GetValue<bool>("Endpoints:Enabled"))
        endpoints.MapControllers();
});

app.MapHealthChecks("/health");

app.Run();


