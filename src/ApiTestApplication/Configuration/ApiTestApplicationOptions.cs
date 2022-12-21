namespace ApiTestApplication.Configuration
{
    public class ApiTestApplicationOptions
    {
        public CorsPolicyOptions? CorsPolicy { get; set; }

        public LocalizationOptions? Localization { get; set; }

        public ResponseCachingOptions? ResponseCaching { get; set; }

        public SwaggerOptions? Swagger { get; set; }
    }
}