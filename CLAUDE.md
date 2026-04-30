# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build the entire solution
dotnet build DSoft.Portable.WebClient.slnx

# Build in Release (generates NuGet packages)
dotnet build DSoft.Portable.WebClient.slnx -c Release

# Run all tests
dotnet test UnitTester/UnitTester.csproj

# Run a specific test class or method
dotnet test UnitTester/UnitTester.csproj --filter "FullyQualifiedName~TestClassName"
```

## Architecture

This is a multi-project .NET library (targets `net462`, `net9.0`, `net10.0`) providing base classes for consuming ASP.NET Core REST APIs and gRPC services. All library projects share build settings from `Directory.Build.props` (NuGet packaging, signing via `DSoft.snk`, SourceLink on Release).

### Package Dependency Graph

```
DSoft.Portable.WebClient.Encryption
    ↑
DSoft.Portable.WebClient.Rest             DSoft.Portable.WebClient.Grpc
    ↑                                           ↑
DSoft.Portable.WebClient.Rest.Encryption  DSoft.Portable.WebClient.Grpc.Encryption
    ↑
DSoft.Portable.WebClient.Rest.Extensions  DSoft.Portable.WebClient.Grpc.Extensions
```

### Core Modules

**`DSoft.Portable.WebClient.Rest`** — primary module. `RestServiceClientBase` is the abstract base all REST clients extend. It handles:
- HTTP operations via RestSharp (`ExecuteGetAsync<T>`, `ExecutePostAsync<T>`, `ExecuteDeleteAsync<T>`)
- URL construction via `CalculateUrlForMethod` (uses `ApiPrefix`, `ControllerName`, and optional module/service segments)
- Authentication lifecycle: `PreflightAsync` → request → `HandleResponseAsync` → `HandleAuthFailureAsync`
- Three auth modes (set on `RestApiClientOptions.AuthenticationType`): `Anonymous`, `Cookie` (via `ICookieManager`), `Token` (via `IJwtTokenManger`)
- `IDisposable` implementation for HTTP client cleanup

**`DSoft.Portable.WebClient.Grpc`** — `GrpcServiceClientBase<T>` (generic) and non-generic variant. The `RPCChannel` property lazily creates a gRPC channel via `GrpcChannelManager`. Supports `Http_1_1` (gRPC-Web) and `Http_2_0` modes via `GrpcClientOptions.GrpcMode`.

**`DSoft.Portable.WebClient.Encryption`** — standalone encryption abstraction. `IEncryptionProvider` defines `Encrypt`/`Decrypt` for strings and bytes. `AesEncryptionProvider` is the default (AES-256 CBC). `EncryptionProviderFactory.Build(ivKey)` returns a configured provider — the IV key must be passed explicitly per call (since v3.1), enabling multi-service scenarios with different IVs.

**`DSoft.Portable.WebClient.Rest.Encryption`** — `RestServiceSecureClientBase` extends the REST base to add encrypted request/response support using `SecurePayload`, `SecureRequest`, and `SecureResponse`.

**`DSoft.Portable.WebClient.Rest.Extensions`** — extension methods on `RestServiceClientBase` for building common secure request patterns (`BuildUserPostRequest`, `BuildSecurePostRequest`, etc.).

### Configuration Objects

`RestApiClientOptions` — passed to `RestServiceClientBase` constructor (via DI or direct):
- `AuthenticationType` — `Anonymous` | `Cookie` | `Token`
- `TimeOut` — default 30 seconds
- `HttpMessageHandler` — inject a custom handler (useful for `WebApplicationFactory` in tests)
- `UrlBuilder` — `Func<string>` for dynamic base URL resolution
- `DefaultHeaders` — added to every request

`GrpcClientOptions` — equivalent for gRPC:
- `GrpcMode` — `Http_1_1` | `Http_2_0`
- `DisableSSLCertValidation` / `ServerCertificateCustomValidationCallback`
- `HttpMessageHandler` — supports `WebApplicationFactory` testing

### Request/Response Pattern

All responses extend `ResponseBase` (`Success: bool`, `Message: string`). Concrete client subclasses must implement:
- `ClientVersionNo` (string property)
- `ControllerName` (string property, maps to API controller route segment)
- Optionally `ApiPrefix` (defaults to `"api"`)

### Testing

Tests live in `UnitTester/` (MSTest, targets `net10.0`). Integration tests use `Microsoft.AspNetCore.Mvc.Testing` (`WebApplicationFactory`). The `SampleWebApp` project is the system under test; `SampleApiClient` contains the client implementation used in tests. Pass a custom `HttpMessageHandler` from the test factory into `RestApiClientOptions` or `GrpcClientOptions` to intercept HTTP traffic without a real server.

### Key Interfaces for Extension

| Interface | Purpose |
|-----------|---------|
| `ICookieManager` | Persist and validate cookies for cookie-auth mode |
| `IJwtTokenManger` | Load tokens and handle auth failure callbacks |
| `IEncryptionProvider` | Pluggable encrypt/decrypt (default: AES-256 CBC) |
| `IIVKeyProvider` | Supply initialization vectors for encryption |
| `IGrpcChannelManager` | Manage gRPC channel lifecycle |
