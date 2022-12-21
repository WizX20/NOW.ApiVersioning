
![Api Versioning Banner](res/banner/banner.png)

**NOW API Versioning, is a set of extensions to simplify REST API versioning**

Simplifies setup and configuration for API Version behavior with Microsoft.AspNetCore.Mvc.Versioning. The middleware provided combines path (URL), query-string and header version-readers. Additionally, a swagger package is provided, which contains extensions to add 'Versioned Swagger' and 'Versioned Swagger UI' documentation.


Build Status
------------

Branch | Status
--- | :---:
master | [![master](https://github.com/WizX20/NOW.ApiVersioning/actions/workflows/ci.yml/badge.svg?branch=master&event=push)](https://github.com/WizX20/NOW.ApiVersioning/actions/workflows/ci.yml)


NuGet Packages
---------------------------

| Package Name | .NET 7 |
| ------------ | :-----------: |
| [ApiVersioning][NOW.ApiVersioning.nuget] | 1.0.1 Preview |
| [ApiVersioning.Swagger][NOW.ApiVersioning.Swagger.nuget] | 1.0.1 Preview |


# Getting started

Clone the repository and run the included *ApiTestApplication*, the default start-page will be the swagger documentation page, where you can play around with the test controller/endpoint, in combination with Api Versioning.

Additionally, you can visit `/allservices`, to display all service registrations. This endpoint
uses the package [Ardalis.ListStartupServices](https://github.com/ardalis/AspNetCoreStartupServices).


# Features

- [NOW.FeatureFlagExtensions.ApiVersioning](src/ApiVersioning/README.md)<br>
  Simplifies setup and configuration for API Version behavior with `Microsoft.AspNetCore.Mvc.Versioning`. The middleware provided combines path (_URL_), query-string and header version-readers. [read more...](src/ApiVersioning/README.md)

- [NOW.FeatureFlagExtensions.ApiVersioning.Swagger](src/ApiVersioning.Swagger/README.md)<br>
  This package contains extensions to add a 'Versioned Swagger' and 'Versioned Swagger UI' documentation, using the `NOW.FeatureFlagExtensions.ApiVersioning` package. [read more...](src/ApiVersioning.Swagger/README.md)


# Community

This project has adopted the code of conduct defined by the [Contributor Covenant](https://contributor-covenant.org/) to clarify expected behavior in our community. For more information, see the [Code of Conduct](docs/CODE_OF_CONDUCT.md).


[NOW.ApiVersioning.nuget]: https://www.nuget.org/packages/NOW.ApiVersioning
[NOW.ApiVersioning.Swagger.nuget]: https://www.nuget.org/packages/NOW.ApiVersioning.Swagger
