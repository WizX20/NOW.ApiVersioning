using Ardalis.ListStartupServices;
using ApiTestApplication;
using ApiTestApplication.Configuration;
using ApiTestApplication.Controllers;
using ApiTestApplication.Extensions.ServiceCollection;
using ApiTestApplication.Extensions.WebHostEnvironment;
using ApiTestApplication.Swagger.OperationFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NOW.ApiTestApplication.Extensions.ServiceCollection;
using NOW.ApiVersioning.Extensions;
using NOW.ApiVersioning.Middleware;
using NOW.ApiVersioning.Swagger.Extensions;

/*
    Start of application start-up.
*/
var builder = WebApplication.CreateBuilder(args);

// Load application settings.
var environment = builder.Environment;
var configuration = environment.GetConfigurationRoot();
var appSettings = builder.Services.RegisterAppSettingsConfiguration(configuration);

// Add services to the container.
builder.Services.AddDefaultControllerOptions();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddEndpointsApiExplorer();

// Add middleware that renders all injected services.
builder.Services.Configure<ServiceConfig>(config =>
{
    config.Services = new List<ServiceDescriptor>(builder.Services);
    config.Path = "/allservices";
});

// Add response caching.
var responseCachingOptions = appSettings.ApiTestApplication?.ResponseCaching;
if (responseCachingOptions != null)
{
    builder.Services.AddResponseCaching(options =>
    {
        options.MaximumBodySize = responseCachingOptions.MaximumBodySize;
        options.UseCaseSensitivePaths = responseCachingOptions.UseCaseSensitivePaths;
    });
}

// Add Api versioning.
var apiVersioningOptions = appSettings.ApiVersioning;
if (apiVersioningOptions != null)
{
    builder.Services.TryAddSingleton(apiVersioningOptions);

    var defaultApiVersion = configuration.GetDefaultApiVersion(
        configurationSection: nameof(AppSettingsConfiguration.ApiVersioning),
        defaultApiVersionFallback: Constants.ApiVersioning.DefaultApiVersion
    );

    builder.Services.AddApiVersioning(options =>
    {
        options.SetDefaultApiVersioningOptions(defaultApiVersion);
        options.Conventions.Controller<TestController>().HasApiVersion(defaultApiVersion);
        options.Conventions.Controller<ApiTestApplication.Controllers.v1_1.TestController>().HasApiVersion(new ApiVersion(1, 1));
    });

    builder.Services.AddVersionedApiExplorer(options =>
    {
        // Add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]".
        options.GroupNameFormat = "'v'VVV";

        // Note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates.
        options.SubstituteApiVersionInUrl = true;
    });
}

// Configure versioned SwaggerGen options.
if (apiVersioningOptions != null)
{
    builder.Services.ConfigureApiVersioningSwaggerGenOptions();
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName);
    options.EnableAnnotations();

    // Configures swagger for support with versioning via the path, query or header.
    if (apiVersioningOptions != null)
    {
        options.SetDefaultApiVersioningOptions();
    }

    // Example filters for file-result and validation.
    options.OperationFilter<FileResultContentTypeOperationFilter>();
    options.OperationFilter<ValidateRequiredParameters>();
});

// Register basic application services.
builder.Services.AddHsts(options =>
{
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddDefaultCorsPolicy(appSettings);

/*
    Start of the HTTP request pipeline configuration.
*/
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    if (apiVersioningOptions != null)
    {
        // Add versioned Swagger docs using the ApiVersioning package.
        app.UseVersionedSwagger();
        app.UseVersionedSwaggerUI();
    }
    else
    {
        // Add default Swagger docs.
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseShowAllServicesMiddleware();
    app.UseDeveloperExceptionPage();
}

// Configure middleware.
app.UseMiddleware<ApiVersionMiddleware>();

// Configure routing.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();