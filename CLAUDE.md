# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Solution layout

`ClassicNET.slnx` (the new XML solution format) contains three projects that span two very different .NET worlds:

- **WebAPI/** — ASP.NET Web API 2 on .NET Framework 4.8.1. Classic non-SDK csproj with `packages.config` NuGet restore (packages land in the checked-in root `packages/` folder). Controllers in `Controllers/`, route/filter/bundle config in `App_Start/`, plus an auto-generated API help site under `Areas/HelpPage/`. Runs under IIS Express on SSL port 44362.
- **WebMVC/** — ASP.NET MVC 5 on .NET Framework 4.8.1, same classic project style. IIS Express SSL port 44329.
- **WebAPI.Tests/** — SDK-style xUnit project targeting **net10.0**. Because of the framework mismatch it **cannot reference the WebAPI project**; existing tests are self-contained placeholders that exercise fake in-memory stand-ins of the controllers. Real coverage of WebAPI code would require multi-targeting a shared library or HTTP-level integration tests.

`Xunit` is a global using in the test project (declared in the csproj), so test files don't need `using Xunit;`.

## Build and test commands

The Framework projects require MSBuild (Visual Studio) — `dotnet build` does not work on them:

```powershell
nuget restore ClassicNET.slnx        # packages.config restore for WebAPI/WebMVC
msbuild ClassicNET.slnx              # requires MSBuild 17.13+ for .slnx support
```

The test project is standard `dotnet` tooling:

```powershell
dotnet test WebAPI.Tests/WebAPI.Tests.csproj                                   # all tests
dotnet test WebAPI.Tests/WebAPI.Tests.csproj --filter "FullyQualifiedName~ValuesControllerTests"   # one class
dotnet test WebAPI.Tests/WebAPI.Tests.csproj --filter "DisplayName~Get_ReturnsAllValues"           # one test
```

Run the web projects via IIS Express from Visual Studio (no `dotnet run`).

## CI

`azure-pipelines.yml` builds on `windows-latest`: NuGet restore → VSBuild (packaged web deploy) → VSTest, triggered on `main`. Note its `solution` variable is `'**/*.sln'`, which does **not** match the `.slnx` file — keep this in mind if pipeline changes are requested.
