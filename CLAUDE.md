# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Test Commands

This project uses **Microsoft Testing Platform (MTP)** with the **TUnit** testing framework. Test commands differ significantly from traditional VSTest.

See: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=dotnet-test-with-mtp

### Prerequisites

```powershell
# Check .NET installation
dotnet --info

# Restore NuGet packages
dotnet restore Splat.slnx
```

**Note**: This repository uses **SLNX** (XML-based solution format) instead of the legacy SLN format.

### Build Commands

**CRITICAL:** The working folder must be `./src` folder. These commands won't function properly without the correct working folder.

```powershell
# Build the solution
dotnet build Splat.slnx -c Release

# Build with warnings as errors (includes StyleCop violations)
dotnet build Splat.slnx -c Release -warnaserror

# Clean the solution
dotnet clean Splat.slnx
```

### Test Commands (Microsoft Testing Platform)

**CRITICAL:** This repository uses MTP configured in `global.json`. All TUnit-specific arguments must be passed after `--`:

The working folder must be `./src` folder. These commands won't function properly without the correct working folder.

**IMPORTANT:**
- Do NOT use `--no-build` flag when running tests. Always build before testing to ensure all code changes (including test changes) are compiled. Using `--no-build` can cause tests to run against stale binaries and produce misleading results.
- Use `--output Detailed` to see Console.WriteLine output from tests. This must be placed BEFORE any `--` separator:
  ```powershell
  dotnet test --output Detailed -- --treenode-filter "..."
  ```

```powershell
# Run all tests in the solution
dotnet test --solution Splat.slnx -c Release

# Run all tests in a specific project
dotnet test --project tests/Splat.Tests/Splat.Tests.csproj -c Release

# Run a single test method using treenode-filter
# Syntax: /{AssemblyName}/{Namespace}/{ClassName}/{TestMethodName}
dotnet test --project tests/Splat.Tests/Splat.Tests.csproj -- --treenode-filter "/*/*/*/MyTestMethod"

# Run all tests in a specific class
dotnet test --project tests/Splat.Tests/Splat.Tests.csproj -- --treenode-filter "/*/*/MyClassName/*"

# Run tests in a specific namespace
dotnet test --project tests/Splat.Tests/Splat.Tests.csproj -- --treenode-filter "/*/MyNamespace/*/*"

# Filter by test property (e.g., Category)
dotnet test --solution Splat.slnx -- --treenode-filter "/*/*/*/*[Category=Integration]"

# Run tests with code coverage (Microsoft Code Coverage)
dotnet test --solution Splat.slnx -- --coverage --coverage-output-format cobertura

# Run tests with detailed output
dotnet test --solution Splat.slnx -- --output Detailed

# List all available tests without running them
dotnet test --project tests/Splat.Tests/Splat.Tests.csproj -- --list-tests

# Fail fast (stop on first failure)
dotnet test --solution Splat.slnx -- --fail-fast

# Control parallel test execution
dotnet test --solution Splat.slnx -- --maximum-parallel-tests 4

# Generate TRX report
dotnet test --solution Splat.slnx -- --report-trx

# Disable logo for cleaner output
dotnet test --project tests/Splat.Tests/Splat.Tests.csproj -- --disable-logo

# Combine options: coverage + TRX report + detailed output
dotnet test --solution Splat.slnx -- --coverage --coverage-output-format cobertura --report-trx --output Detailed
```

**Alternative: Using `dotnet run` for single project**
```powershell
# Run tests using dotnet run (easier for passing flags)
dotnet run --project tests/Splat.Tests/Splat.Tests.csproj -c Release -- --treenode-filter "/*/*/*/MyTest"

# Disable logo for cleaner output
dotnet run --project tests/Splat.Tests/Splat.Tests.csproj -- --disable-logo --treenode-filter "/*/*/*/Test1"
```

### TUnit Treenode-Filter Syntax

The `--treenode-filter` follows the pattern: `/{AssemblyName}/{Namespace}/{ClassName}/{TestMethodName}`

**Examples:**
- Single test: `--treenode-filter "/*/*/*/MyTestMethod"`
- All tests in class: `--treenode-filter "/*/*/MyClassName/*"`
- All tests in namespace: `--treenode-filter "/*/MyNamespace/*/*"`
- Filter by property: `--treenode-filter "/*/*/*/*[Category=Integration]"`
- Multiple wildcards: `--treenode-filter "/*/*/MyTests*/*"`

**Note:** Use single asterisks (`*`) to match segments. Double asterisks (`/**`) are not supported in treenode-filter.

### Key TUnit Command-Line Flags

- `--treenode-filter` - Filter tests by path pattern or properties (syntax: `/{Assembly}/{Namespace}/{Class}/{Method}`)
- `--list-tests` - Display available tests without running
- `--fail-fast` - Stop after first failure
- `--maximum-parallel-tests` - Limit concurrent execution (default: processor count)
- `--coverage` - Enable Microsoft Code Coverage
- `--coverage-output-format` - Set coverage format (cobertura, xml, coverage)
- `--report-trx` - Generate TRX format reports
- `--output` - Control verbosity (Normal or Detailed)
- `--no-progress` - Suppress progress reporting
- `--disable-logo` - Remove TUnit logo display
- `--diagnostic` - Enable diagnostic logging (Trace level)
- `--timeout` - Set global test timeout
- `--reflection` - Enable reflection mode instead of source generation

See https://tunit.dev/docs/reference/command-line-flags for complete TUnit flag reference.

### Key Configuration Files

- `global.json` - Specifies `"Microsoft.Testing.Platform"` as the test runner
- `testconfig.json` - Configures test execution (`"parallel": true`) and code coverage (Cobertura format)
- `Directory.Build.props` - Enables `TestingPlatformDotnetTestSupport` for test projects
- `.github/COPILOT_INSTRUCTIONS.md` - Comprehensive development guidelines

## Architecture Overview

### Core Framework Structure

Splat is a cross-platform utility library providing abstractions for common functionality that should work everywhere but often doesn't. It solves the problem of writing cross-platform mobile and desktop code riddled with `#ifdefs`.

**Core Library (`Splat/`)**
- `ServiceLocation/` - Simple, flexible service location/dependency injection
- `AppLocator` - Main service locator for registering and retrieving services
- `PlatformModeDetector/` - Detect unit test runners and design mode
- Core abstractions and utilities

**Specialized Libraries**
- `Splat.Builder/` - AOT-friendly configuration with `AppBuilder` and `IModule` pattern
- `Splat.Core/` - Core interfaces and abstractions
- `Splat.Logging/` - Cross-platform logging abstraction with `ILogger` interface
- `Splat.Drawing/` - Cross-platform image loading/saving and color/geometry primitives

**Dependency Injection Adapters (`Splat.*/`)**
- `Splat.Autofac/` - Autofac container integration
- `Splat.DryIoc/` - DryIoc container integration
- `Splat.Microsoft.Extensions.DependencyInjection/` - Microsoft.Extensions.DependencyInjection integration
- `Splat.Ninject/` - Ninject container integration
- `Splat.SimpleInjector/` - SimpleInjector container integration
- `Splat.Prism/` - Prism framework integration

**Logging Adapters (`Splat.*/`)**
- `Splat.Serilog/` - Serilog logger integration
- `Splat.NLog/` - NLog logger integration
- `Splat.Log4Net/` - Log4Net logger integration
- `Splat.Microsoft.Extensions.Logging/` - Microsoft.Extensions.Logging integration

**Application Performance Monitoring (`Splat.*/`)**
- `Splat.AppCenter/` - Microsoft App Center APM integration
- `Splat.ApplicationInsights/` - Azure Application Insights integration
- `Splat.Exceptionless/` - Exceptionless integration
- `Splat.Raygun/` - Raygun integration

### Key Architectural Patterns

**Service Location**
- `AppLocator.Current` - Retrieve registered services
- `AppLocator.CurrentMutable` - Register new services at runtime
- `RegisterLazySingleton` - Register singletons that are created on first access
- `RegisterConstant` - Register singleton instances
- `Register` - Register factory functions for transient instances

**Logging**
- `IEnableLogger` - Tag interface for classes that want logging support
- `this.Log().Info()` / `this.Log().Warn()` - Extension methods for logging
- `LogHost.Default` - Static logger for static methods
- Platform-specific adapters forward to chosen logging framework

**Cross-Platform Drawing**
- `IBitmap` - Platform-agnostic bitmap interface
- `ToNative()` / `FromNative()` - Convert between Splat and platform types
- `BitmapLoader.Current` - Load/save images from streams, resources, or files
- `SplatColor` - Cross-platform color abstraction
- `PointF`, `SizeF`, `RectangleF` - Cross-platform geometry primitives

**AppBuilder and IModule (AOT-Friendly)**
- `IModule` - Define reusable service registration modules
- `AppBuilder` - Compose modules and apply to resolver
- `UseCurrentSplatLocator()` - Target current `AppLocator.CurrentMutable`
- AOT-safe configuration without reflection

### Multi-Platform Target Framework Strategy

The project uses granular target framework definitions in `Directory.Build.props`:

- `SplatModernTargets` - net8.0, net9.0, net10.0 (modern cross-platform)
- `SplatLegacyTargets` - net462, net472, net481 (Windows-only legacy .NET Framework)
- `SplatCoreTargets` - Combines modern + legacy targets for core libraries
- `SplatWindowsTargets` - net8.0/9.0/10.0-windows10.0.17763.0 and windows10.0.19041.0
- `SplatAndroidTargets` - net9.0-android, net10.0-android
- `SplatAppleTargets` - iOS, tvOS, macOS, Mac Catalyst
- `SplatUiFinalTargetFrameworks` - OS-aware composition for UI projects

**OS-Aware Builds:** The build system automatically selects appropriate target frameworks based on the host OS:
- **Linux**: Builds .NET 8/9/10 and Android targets
- **Windows**: Builds all targets including .NET Framework, Core, Android, Windows, and Apple
- **macOS**: Builds .NET 8/9/10, Android, and Apple targets

### Zero-Reflection and AOT (Ahead-of-Time) Compilation Priority

**CRITICAL: Prefer Zero-Reflection Solutions First**

When implementing new features, follow this priority order:

1. **Zero-reflection solutions** - Design APIs that don't require reflection at all
2. **Source generators** - Use Roslyn source generators to generate code at compile-time
3. **Compile-time code generation** - Use tools like T4 templates or build-time code generation
4. **Strongly-typed expressions** - Use expression trees that can be analyzed without runtime reflection
5. **Reflection with AOT attributes** - Only as a last resort, and always with proper `DynamicallyAccessedMembers` attributes

**Examples of Preferred Approaches:**

**Good: Zero-Reflection with Explicit Registration**
```csharp
// Users explicitly register their services - no reflection needed
AppLocator.CurrentMutable.Register<IMyService>(() => new MyService());
```

**Good: Source Generator Pattern**
```csharp
// Use source generators to generate registration code at compile-time
// See: Splat.DI.SourceGenerator package
[RegisterService(typeof(IMyService))]
public partial class MyService : IMyService
{
    // Generator creates registration code automatically
}
```

**Good: Expression-Based with Static Analysis**
```csharp
// Expression trees can be analyzed without runtime reflection
public void RegisterService<T>(Expression<Func<T>> factory)
{
    // Analyze expression at compile-time or with source generators
}
```

**Avoid: Runtime Reflection (unless properly attributed)**
```csharp
// Only use reflection when absolutely necessary and with proper attributes
#if NET6_0_OR_GREATER
private static void RegisterFromType(
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    Type serviceType)
#else
private static void RegisterFromType(Type serviceType)
#endif
{
    var instance = Activator.CreateInstance(serviceType);
}
```

### AOT Compatibility Requirements

All code targeting net8.0+ should be AOT-compatible:

**Key AOT Patterns:**
- **Always prefer zero-reflection solutions** - design APIs that work without reflection
- **Use source generators** for code that would traditionally require reflection
- Prefer `DynamicallyAccessedMembersAttribute` over `UnconditionalSuppressMessage`
- Use specific `DynamicallyAccessedMemberTypes` values rather than `All` when possible
- `AppBuilder` and `IModule` pattern for AOT-safe dependency injection configuration
- Avoid reflection in hot paths
- Document any reflection usage and why it's necessary
- See `tests/Splat.Aot.Tests/` for AOT test examples

**Source Generator References:**
- Splat.DI.SourceGenerator: https://github.com/reactivemarbles/Splat.DI.SourceGenerator
- .NET Source Generators: https://learn.microsoft.com/dotnet/csharp/roslyn-sdk/source-generators-overview

See `.github/COPILOT_INSTRUCTIONS.md` for comprehensive AOT patterns and examples.

## Code Style & Quality Requirements

**CRITICAL:** All code must comply with ReactiveUI contribution guidelines: https://www.reactiveui.net/contribute/index.html

### Style Enforcement

- EditorConfig rules (`.editorconfig`) - comprehensive C# formatting and naming conventions
- StyleCop Analyzers - builds fail on violations
- Roslynator Analyzers - additional code quality rules
- Analysis level: latest with enhanced .NET analyzers
- `WarningsAsErrors`: nullable
- **All public APIs require XML documentation comments** (including protected methods of public classes)

### C# Style Rules

- **Braces:** Allman style (each brace on new line)
- **Indentation:** 4 spaces, no tabs
- **Fields:** `_camelCase` for private/internal, `readonly` where possible, `static readonly` (not `readonly static`)
- **Visibility:** Always explicit (e.g., `private string _foo` not `string _foo`), visibility first modifier
- **Namespaces:** File-scoped preferred, imports outside namespace, sorted (system then third-party)
- **Types:** Use keywords (`int`, `string`) not BCL types (`Int32`, `String`)
- **Modern C#:** Use nullable reference types, pattern matching, switch expressions, records, init setters, target-typed new, collection expressions, file-scoped namespaces, primary constructors
- **Avoid `this.`** unless necessary
- **Use `nameof()`** instead of string literals
- **Use `var`** when it improves readability or aids refactoring

See `.github/COPILOT_INSTRUCTIONS.md` for complete style guide.

## Testing Guidelines

- Unit tests use **TUnit** framework with **Microsoft Testing Platform**
- Test projects detected via naming convention (project name contains `Tests`)
- Coverage configured in `testconfig.json` (Cobertura format, skip auto-properties)
- Parallel test execution enabled (`"parallel": true` in testconfig.json)
- Always write unit tests for new features or bug fixes
- Follow existing test patterns in `tests/Splat.Tests/`
- For AOT scenarios, reference patterns in `tests/Splat.Aot.Tests/`
- Platform-specific test runners available for Android and UWP

## Common Tasks

### Adding a New Feature

1. **Design for zero-reflection first** - avoid reflection if at all possible
2. **Consider source generators** - use compile-time code generation when needed
3. Create failing tests first
4. Implement minimal functionality
5. Ensure AOT compatibility
6. Update documentation if needed
7. Add XML documentation to all public APIs
8. Prefer to create new methods than changing the signature of existing ones when changing APIs
9. Run formatting validation before committing

### Fixing Bugs

1. Create reproduction test
2. Fix with minimal changes
3. Verify AOT compatibility
4. Ensure no regression in existing tests

### Adding a New Logging or DI Adapter

1. Create new project following naming convention (e.g., `Splat.NewContainer`)
2. Implement adapter interface (`IMutableDependencyResolver` for DI, logger factory for logging)
3. **Prefer zero-reflection implementations** - design adapters to work without runtime reflection
4. Create corresponding test project (e.g., `Splat.NewContainer.Tests`)
5. Add extension methods for registration (e.g., `UseNewContainer()`)
6. Update README.md with new adapter information
7. Ensure adapter works with `AppBuilder` pattern for AOT scenarios

## What to Avoid

- **Runtime reflection** - prefer zero-reflection solutions or source generators
- **Implicit service discovery** - explicit registration is better for AOT
- **Heavy reflection** without proper AOT suppression attributes (if reflection is absolutely necessary)
- **Platform-specific code** in core Splat library (use platform extensions instead)
- **Breaking changes** to public APIs without proper versioning
- **Large dependencies** in core libraries (keep Splat lightweight)
- **Implicit service registrations** that might surprise users or break AOT

## Important Notes

- **Zero-Reflection First:** Always design for zero-reflection solutions before considering alternatives
- **Source Generators:** Leverage source generators for compile-time code generation instead of runtime reflection
- **No shallow clones:** Repository requires full clone for git version information used by Nerdbank.GitVersioning
- **Required .NET SDKs:** .NET 8.0, 9.0, and 10.0 (all three required for full build)
- **SLNX Format:** Uses modern XML-based solution format instead of legacy SLN
- **Comprehensive Instructions:** `.github/COPILOT_INSTRUCTIONS.md` contains detailed development guidelines
- **Code Formatting:** Always run `dotnet format whitespace` and `dotnet format style` before committing

**Philosophy:** Splat provides "leaky abstractions" that solve cross-platform problems while always allowing access to native platform types via `ToNative()` and `FromNative()`. Keep abstractions minimal and focused on solving real cross-platform pain points. **Prefer zero-reflection solutions and source generators over runtime reflection.** When in doubt, prefer simplicity over feature completeness, explicit registration over implicit discovery, and always consider the AOT implications of your changes.
