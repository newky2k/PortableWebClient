# Portable Web Client

Cross platform web client framework for calling ASP.NET Core RESTful Web Apis and Grpc services

## Functionality

The libary provides base level, bootstrap base classes to simplify the building of client libraries for use with ASP.NET Core Web Apis and now Grpc services too.

- Base web service client classes for RESTful Web APIs and Grpc service clients
  - Core Web Client functionality
  - Stores Url
  - Can check for connectivity
  - Stores Default Timeout
- Base service client classes for both RESTFul and Grpc services
  - Handles RestClient and Grpc channel management functionality
  - Helper methods for Get and Post method calls
  - Simplifies calls to the services
- Base request and response classes
- Generic methods and extensions for calling RESTful Web APIs
- EF Core session
  - Secure sessions for users
- Secure request and response classes using a `SecurePayload`
- Integrated encryption, with abiltiy provide override with custom implementations.
  - Uses 256-bit AES encryption by default

## V3.x breaking changes
With the move to v3 some of the classes have been renamed and moved into different packages, such as `ServiceClientBase` which now exists in `DSoft.Portable.WebClient.Rest` and is now named `RestServiceClientBase`.  

Addtionally you know have to set an InitVector key of your own on `EncryptionProviderFactory` before encrption and decryption will function.

We will add a detailed migration guide to the Wiki before going stable

## Build status
[![Build Status](https://dev.azure.com/humbatt/Daves%20Projects/_apis/build/status/PortableWebClient/PortableWebClient%20-%20Release?branchName=master)](https://dev.azure.com/humbatt/Daves%20Projects/_build/latest?definitionId=49&branchName=master)


## Packages

Platform/Feature               | Package name                              | Stable                              | Beta
-----------------------|-------------------------------------------|------------------------------------------------|----------------
Core             | `DSoft.Portable.WebClient.Core` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Core.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Core/) | [![NuGet](https://img.shields.io/nuget/vpre/DSoft.Portable.WebClient.Core.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Core/) |
Encryption             | `DSoft.Portable.WebClient.Encryption` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Encryption.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Encryption/) |  [![NuGet](https://img.shields.io/nuget/vpre/DSoft.Portable.WebClient.Encryption.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Encryption/) |
Grpc             | `DSoft.Portable.WebClient.Grpc` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Grpc.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Grpc/) |[![NuGet](https://img.shields.io/nuget/vpre/DSoft.Portable.WebClient.Grpc.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Grpc/) |
Grpc Encryption       | `DSoft.Portable.WebClient.Grpc.Encryption` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Grpc.Encryption.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Grpc.Encryption/) | [![NuGet](https://img.shields.io/nuget/vpre/DSoft.Portable.WebClient.Grpc.Encryption.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Grpc.Encryption/) |
Grpc Encryption Build Tools | `DSoft.Portable.WebClient.Grpc.Encryption.Tools` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Grpc.Encryption.Tools.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Grpc.Encryption.Tools/) | [![NuGet](https://img.shields.io/nuget/vpre/DSoft.Portable.WebClient.Grpc.Encryption.Tools.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Grpc.Encryption.Tools/) |
Rest             | `DSoft.Portable.WebClient.Rest` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Rest.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Rest/) | [![NuGet](https://img.shields.io/nuget/vpre/DSoft.Portable.WebClient.Rest.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Rest/) |
Rest Encryption          | `DSoft.Portable.WebClient.Rest.Encryption` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Rest.Encryption.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Rest.Encryption/) | [![NuGet](https://img.shields.io/nuget/vpre/DSoft.Portable.WebClient.Rest.Encryption.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Rest.Encryption/) |
Rest Extensions          | `DSoft.Portable.WebClient.Rest.Extensions` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.WebClient.Rest.Extensions.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Rest.Extensions/) | [![NuGet](https://img.shields.io/nuget/vpre/DSoft.Portable.WebClient.Rest.Extensions.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.WebClient.Rest.Extensions/) |
EF Core Security Entities       | `DSoft.Portable.Server.Security.Core` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.Server.Security.Core.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.Server.Security.Core/) | [![NuGet](https://img.shields.io/nuget/vpre/DSoft.Portable.Server.Security.Core.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.Server.Security.Core/) |
EF Core Secure Database contexts           | `DSoft.Portable.EntityFrameworkCore.Security` | [![NuGet](https://img.shields.io/nuget/v/DSoft.Portable.EntityFrameworkCore.Security.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.EntityFrameworkCore.Security/) |  [![NuGet](https://img.shields.io/nuget/vpre/DSoft.Portable.EntityFrameworkCore.Security.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Portable.EntityFrameworkCore.Security/) |
  
