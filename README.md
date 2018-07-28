# NLayerAppV3

This N-Layered Architecture with .Net Core 2.1 project (NLayerAppV3) is retro rebuild new code project based on DDD N-LayeredArchitecture Version 2.
It contains all DDD Layers where the developer and software architect may reuse to implement a .Net Core WebApi (Version 2.1) for Banking and Blog Contexts.
Why to use .NET Core 2.1? We need to use this version because of EF Core Complex Types - Value Object implementation (OwnsOne) and TransactionScope implementation using in Application Layer.
This project is a tribute to the recent visit to Madrid of Cesar de la Torre in order to give us an amazing conference called 'Microservices Architectures' [https://geeks.ms/plainnews/2017/05/04/microservices-architectures/].

## Demo

- Main Bounded Context [http://nlayerappv3mainboundedcontext.azurewebsites.net]
- Blog Bounded Context [http://nlayerappv3blogboundedcontext.azurewebsites.net]

## Getting Started

Install the .NET Core 2.1 runtime for your host environment from [https://www.microsoft.com/net/core/] (https://www.microsoft.com/net/core/)

In a terminal, navigate to the folder of this project and type the following to restore the dependencies:

```
dotnet restore
```

To start the API, navigate to the 'DistributedServices.MainBoundedContext' folder and type:

```
dotnet run
```

Browse to http://localhost:5000/ to see the result.


To run the tests, navigate to the 'DistributedServices.MainBoundedContext.Tests' or antoher test project folder and type:

```
dotnet test
```

## Code Overview

The solution consists of sixteen projects: the API, Application, Domain and infrastructure projects.

- DistributedServices.MainBoundedContext
- DistributedServices.MainBoundedContext.Tests
- Application.Seedwork
- Application.MainBoundedContext
- Application.MainBoundedContext.DTO
- Application.MainBoundedContext.Tests
- Domain.Seedwork
- Domain.Seedwork.Tests
- Domain.MainBoundedContext
- Domain.MainBoundedContext.Tests
- Infrastructure.Data.Seedwork
- Infrastructure.Data.MainBoundedContext
- Infrastructure.Data.MainBoundedContext.Tests
- Infrastructure.Crosscutting
- Infrastructure.Crosscutting.NetFramework
- Infrastructure.Crosscutting.Tests

### DistributedServices.MainBoundedContext

This project contains the API controllers (sync and async methods).

#### Dependencies
- Microsoft.NETCore.App: A set of .NET API's that are included in the default .NET Core application model. 

### DistributedServices.MainBoundedContext.Tests

This project contains the integration and unit tests.

#### Dependencies
- TestHost: for in-process API testing
- xUnit: as the testing framework
- Newtonsoft.Json: for JSON deserialization
- FluentAssertions: to write beautiful assertions with Should().
- Moq: Moq is intended to be simple to use, strongly typed and minimalistic (while still fully functional!).

### Application.Seedwork

This project contains Error Exceptions and Projections Extensions.

#### Dependencies
- Microsoft.NETCore.App: A set of .NET API's that are included in the default .NET Core application model. 

### Application.MainBoundedContext

This project contains the modules and an anemic generic application common service.

#### Dependencies
- Microsoft.Extensions.Logging: Logging. 

### Application.MainBoundedContext.DTO

This project contains the DTOs, profiles and validations.

#### Dependencies
- Automapper: A convention-based object-object mapper
- Fluent Validation: A small validation library for .NET
- Newtonsoft.Json: for JSON deserialization

### Application.MainBoundedContext.Tests

This project contains the adapters and application services tests.

#### Dependencies
- xUnit: as the testing framework
- Moq: Moq is intended to be simple to use, strongly typed and minimalistic (while still fully functional!).

### Domain.Seedwork

This project contains the base entities with guid, int and string identities, value object and auditable classes.

#### Dependencies
- Microsoft.NETCore.App: A set of .NET API's that are included in the default .NET Core application model. 

### Domain.Seedwork.Tests

This project contains unit tests.

#### Dependencies
- xUnit: as the testing framework

### Domain.MainBoundedContext

This project contains the aggregates, domains entities, value objects, factories, specifications and the repositories interfaces.

#### Dependencies
- Microsoft.NETCore.App: A set of .NET API's that are included in the default .NET Core application model. 

### Domain.MainBoundedContext.Tests

This project contains unit tests of the domain entities, value objects, ....

#### Dependencies
- xUnit: as the testing framework

### Infrastructure.Data.Seedwork

This project contains the Repository base class and some interfaces.

#### Dependencies
- Microsoft.EntityFrameworkCore.InMemory: In-memory database provider for Entity Framework Core (to be used for testing purposes). 

### Infrastructure.Data.MainBoundedContext

This project contains the repositories.

#### Dependencies
- Microsoft.EntityFrameworkCore.InMemory: In-memory database provider for Entity Framework Core (to be used for testing purposes).
- System.Security.Claims: Provides classes that implement claims-based identity.

### Infrastructure.Data.MainBoundedContext.Tests

This project contains the repository unit tests.

#### Dependencies
- xUnit: as the testing framework

### Infrastructure.Crosscutting

This project contains the Adapters, Localization and Validator Crosscutting definitions.

#### Dependencies
- Microsoft.NETCore.App: A set of .NET API's that are included in the default .NET Core application model. 

### Infrastructure.Crosscutting.NetFramework

This project contains the Adapters, Localization and Validator Crosscutting factory implementations.

#### Dependencies
- Microsoft.NETCore.App: A set of .NET API's that are included in the default .NET Core application model. 

### Infrastructure.Crosscutting.Tests

This project contains the crosscutting unit tests.

#### Dependencies
- xUnit: as the testing framework

## Documentation

- Announcing .NET Core 2.0: [https://blogs.msdn.microsoft.com/dotnet/2017/08/14/announcing-net-core-2-0/](https://blogs.msdn.microsoft.com/dotnet/2017/08/14/announcing-net-core-2-0/)
- .NET Core 2.0 Preview 2 Release Notes: [https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0.0-preview2.md](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0.0-preview2.md)
- 2.0.0 Preview 2 Known Issues: [https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0.0-preview2-known-issues.md](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0.0-preview2-known-issues.md)
- .NET Core Roadmap: [https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0.0-preview1-known-issues.md](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0.0-preview1-known-issues.md)
- .NET Core 2.0 Changes – Things to Know: [https://stackify.com/net-core-2-0-changes/](https://stackify.com/net-core-2-0-changes/)
- Our brand-new ‘DDD N-Layered .NET 4.0 Architecture Guide’ book and Sample-App in CODEPLEX: [https://blogs.msdn.microsoft.com/cesardelatorre/2010/03/25/our-brand-new-ddd-n-layered-net-4-0-architecture-guide-book-and-sample-app-in-codeplex/](https://blogs.msdn.microsoft.com/cesardelatorre/2010/03/25/our-brand-new-ddd-n-layered-net-4-0-architecture-guide-book-and-sample-app-in-codeplex/)

## Things to improve

- .Net Core Localization.
- Docker.
- FrontEnd implementation with Angular.
- More tests.
- Store Procedures execution with ef core.
- Versioning.
- Security with Identity Server4.
- Cache.

## Feedback

Feedback about this project is greatly appreciated.

## Copyright

2017 César Castro and Microsoft Corporation
