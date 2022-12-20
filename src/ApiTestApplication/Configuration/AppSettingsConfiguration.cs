using NOW.ApiVersioning.Configuration;

namespace ApiTestApplication.Configuration
{
    public class AppSettingsConfiguration
    {
        public ApiTestApplicationOptions? ApiTestApplication { get; set; }

        public ApiVersioningOptions? ApiVersioning { get; set; }
    }
}