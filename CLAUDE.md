# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Solution layout

`ClassicNET.slnx` (the new XML solution format) contains five projects that straddle two .NET worlds — classic .NET Framework 4.8.1 web apps and modern net10.0 test projects:

- **WebAPI/** — ASP.NET Web API 2 on .NET Framework 4.8.1. Classic non-SDK csproj with `packages.config` NuGet restore (packages land in the checked-in root `packages/` folder). Controllers in `Controllers/`, route/filter/bundle config in `App_Start/`, EF model in `Models/`, entity→DTO mapping in `Mapping/`, plus an auto-generated API help site under `Areas/HelpPage/`. IIS Express SSL port 44362.
- **WebMVC/** — ASP.NET MVC 5 on .NET Framework 4.8.1, same classic project style. Scaffolded CRUD controllers + Razor views, EF model in `Models/`, HtmlHelper extensions in `Helpers/`. IIS Express SSL port 44329.
- **Shared.Contracts/** — SDK-style class library targeting **net481** holding plain DTO types under `DTOs/`. This is the one project both web apps reference (a net481 SDK lib *can* be referenced by classic net481 projects), so it's the seam for sharing types between WebAPI and WebMVC without sharing EF entities.
- **WebAPI.Tests/** and **WebMVC.Tests/** — SDK-style xUnit projects targeting **net10.0**. Because of the framework mismatch they **cannot reference the WebAPI/WebMVC projects**; existing tests are self-contained placeholders that mirror controller/route logic against in-memory stand-ins. Real coverage would require multi-targeting a shared library or HTTP-level integration tests.

`Xunit` is a global using in both test projects (declared in the csproj `<Using>` items), so test files don't need `using Xunit;`. **WebMVC.Tests also globally imports `FluentAssertions`** (and uses the `.Should()` style); WebAPI.Tests does not.

## Data layer

Both WebAPI and WebMVC use **Entity Framework 6 database-first** against a local SQL Server Express database named **MVCNet** (a Northwind variant). Connection string `MVCNetEntities` in each `Web.config` points at `.\SQLEXPRESS` / catalog `MVCNet` — running either web app requires that instance and database to exist locally.

Each web project has its **own private copy** of the model (`Models/DataModel.edmx` + the T4-generated entity partials and `MVCNetEntities` DbContext). The two copies are not shared — a schema change must be re-applied in both. Entities are generated `partial class`es, so **validation/formatting annotations live in separate `[MetadataType]` buddy classes** (e.g. `CategoryMetadata.cs`, `OrderMetadata.cs`) to survive model regeneration — add annotations there, never in the generated files.

- **WebMVC** controllers `new` up `MVCNetEntities` directly, page with `Skip/Take` + `ViewBag`, and build dropdowns via a `PopulateDropdowns`/`SelectList` helper. `Helpers/PictureHelper.cs` strips the 78-byte OLE header off Northwind category `Picture` blobs before rendering them as data URIs.
- **WebAPI** controllers disable proxy creation / lazy loading, then project EF entities to `Shared.Contracts` DTOs through the `ToDto()`/`ToDtoList()` extension methods in `Mapping/DtoMapper.cs`, returning a `{ items, page, pageSize, totalCount, totalPages }` envelope.

## Build and test commands

The Framework projects require MSBuild (Visual Studio) — `dotnet build` does not work on them:

```powershell
nuget restore ClassicNET.slnx        # packages.config restore for WebAPI/WebMVC
msbuild ClassicNET.slnx              # requires MSBuild 17.13+ for .slnx support
```

The test projects use standard `dotnet` tooling:

```powershell
dotnet test WebAPI.Tests/WebAPI.Tests.csproj                                   # one test project
dotnet test WebMVC.Tests/WebMVC.Tests.csproj
dotnet test WebAPI.Tests/WebAPI.Tests.csproj --filter "FullyQualifiedName~ValuesControllerTests"   # one class
dotnet test WebAPI.Tests/WebAPI.Tests.csproj --filter "DisplayName~Get_ReturnsAllValues"           # one test
```

Run the web projects via IIS Express from Visual Studio (no `dotnet run`).

## CI

`azure-pipelines.yml` builds on `windows-latest`: NuGet restore → VSBuild (packaged web deploy) → VSTest, triggered on `main`. Note its `solution` variable is `'**/*.sln'`, which does **not** match the `.slnx` file — keep this in mind if pipeline changes are requested.

<!-- SPECKIT START -->
For additional context about technologies to be used, project structure,
shell commands, and other important information, read the current plan
<!-- SPECKIT END -->

## Instructions
If a question involves any Microsoft product, service, or technology
(Azure, .NET, SQL Server, ASP.NET, C#, EF Core, etc.),
always use the Microsoft Learn MCP tools to search for up-to-date
documentation before answering. Cite the source URL in your response.