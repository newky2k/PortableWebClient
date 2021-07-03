# Portable Web Client

Portable Client Framework for calling ASP.NET Core RESTful Web Apis and Grpc services

## Functionality

The libary provides base level, bootstrap base classes to simplify the building of client libraries for use with ASP.NET Core Web Apis and now Grpc services too.

- Base web service client classes for RESTful Web APIs and Grpc service clients
  - Core Web Client functionality
  - Stores Url
  - Can check for connectivity
  - Stores Default Timeout
- Base service client classes for both RESTFul and Grpc services
  - Handles RestClient functionality
  - Helper methods for Get and Post method calls
  - Simplifies calls to the services
- Base request and response classes
- Generic methods and extensions for calling RESTful Web APIs
- EF Core session
  - Secure sessions for users
- Secure request and response classes using a `SecurePayload`

## Build status

![alt-text](https://dev.azure.com/humbatt/Daves%20Projects/_apis/build/status/PortableWebClient/PortableWebClient%20Build "DevOps")

## Packages

Platform/Feature               | Package name                              | Stable
-----------------------|-------------------------------------------|-----------------------------
Core             | `DSoft.Portable.WebClient.Core` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Core.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Core/) |
WebClient             | `DSoft.Portable.WebClient` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient/) |
Encryption             | `DSoft.Portable.WebClient.Encryption` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Encryption.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Encryption/) |
Extensions             | `DSoft.Portable.WebClient.Extensions` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Extensions.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Extensions/) |
EF Core Security Entities       | `DSoft.Portable.Server.Security.Core` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.Server.Security.Core.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.Server.Security.Core/) |
EF Core Secure Database contexts           | `DSoft.Portable.EntityFrameworkCore.Security` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.EntityFrameworkCore.Security.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.EntityFrameworkCore.Security/) |
  