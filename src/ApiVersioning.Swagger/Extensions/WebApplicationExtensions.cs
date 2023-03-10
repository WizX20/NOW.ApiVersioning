using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NOW.ApiVersioning.Swagger.Extensions
{
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// Default JSON route used when none was found in the active configuration.
        /// </summary>
        private const string DefaultJsonRoute = "swagger/{documentName}/swagger.json";

        /// <summary>
        /// Activates a versioned swagger configuration.
        /// </summary>
        public static void UseVersionedSwagger(
            this WebApplication app,
            string? jsonRoute = null,
            ILogger? logger = null)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (string.IsNullOrWhiteSpace(jsonRoute))
            {
                logger?.LogWarning("The {JsonRoute} is empty, using default: {DefaultJsonRoute}.",
                    nameof(jsonRoute), DefaultJsonRoute);

                jsonRoute = DefaultJsonRoute;
            }

            app.UseSwagger(options =>
                options.RouteTemplate = jsonRoute
            );
        }

        /// <summary>
        /// Activates a versioned swagger configuration.
        /// </summary>
        public static void UseVersionedSwaggerUI(
            this WebApplication app,
            string? jsonRoute = null,
            ILogger? logger = null)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            if (apiVersionDescriptionProvider is null)
            {
                throw new ArgumentNullException(nameof(apiVersionDescriptionProvider));
            }

            if (string.IsNullOrWhiteSpace(jsonRoute))
            {
                logger?.LogWarning("The {JsonRoute} is empty, using default: {DefaultJsonRoute}.",
                    nameof(jsonRoute), DefaultJsonRoute);

                jsonRoute = DefaultJsonRoute;
            }

            app.UseSwaggerUI(options =>
            {
                options.DisplayOperationId();
                options.DisplayRequestDuration();

                var swaggerJsonBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
                foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    var apiVersion = apiVersionDescription.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(
                        $"{swaggerJsonBasePath}/swagger/{apiVersionDescription.GroupName}/swagger.json",
                        apiVersion
                    );

                    logger?.LogInformation("A swagger endpoint for version '{Version}' was added.", apiVersion);
                }
            });
        }
    }
}