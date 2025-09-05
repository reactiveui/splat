# Splat: Cross-Platform Library for .NET

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Prerequisites and Environment Setup
- **CRITICAL**: Requires .NET 9.0 SDK (not .NET 8.0). Install with:
  ```bash
  curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version latest --channel 9.0
  export PATH="$HOME/.dotnet:$PATH"
  ```
- **Platform Support**: This project builds on Windows, Linux, and macOS with cross-platform support.
- **Development Tools**: Visual Studio 2022, VS Code with C# extension, or JetBrains Rider.
- **Note on Cloning the Repository**:
  When cloning the Splat repository, use a full clone instead of a shallow one (e.g., avoid --depth=1). This project uses Nerdbank.GitVersioning for automatic version calculation based on Git history. Shallow clones lack the necessary commit history, which can cause build errors or force the tool to perform an extra fetch step to deepen the repository. To ensure smooth builds:
   ```bash
   git clone https://github.com/reactiveui/splat.git
   ```
  If you've already done a shallow clone, deepen it with:
   ```bash
   git fetch --unshallow
   ```
  This prevents exceptions like "Shallow clone lacks the objects required to calculate version height."

### Development Workflow
- Full solution restore and build:
  ```bash
  cd src
  dotnet restore Splat.sln
  dotnet build Splat.sln --configuration Release
  ```
  Build time: **2-5 minutes**. Set timeout to 10+ minutes for full solution builds.

- Individual project builds (faster for development):
  ```bash
  cd src
  dotnet build Splat/Splat.csproj --configuration Release
  dotnet build Splat.Tests/Splat.Tests.csproj --configuration Release
  ```

### Testing
- **Full test suite**:
  ```bash
  cd src
  dotnet test --configuration Release
  ```
  Test time: **2-5 minutes**. Set timeout to 15+ minutes.

- **Individual project tests** (faster for development):
  ```bash
  cd src
  dotnet test Splat.Tests/Splat.Tests.csproj --configuration Release
  ```

## Validation and Quality Assurance

### Code Style and Analysis Enforcement
- **EditorConfig Compliance**: Repository uses a comprehensive `.editorconfig` with detailed rules for C# formatting, naming conventions, and code analysis.
- **StyleCop Analyzers**: Enforces consistent C# code style with `stylecop.analyzers`.
- **Roslynator Analyzers**: Additional code quality rules with `Roslynator.Analyzers`.
- **Analysis Level**: Set to `latest` with enhanced .NET analyzers enabled.
- **CRITICAL**: All code must comply with ReactiveUI contribution guidelines: [https://www.reactiveui.net/contribute/index.html](https://www.reactiveui.net/contribute/index.html).

## C# Style Guide
**General Rule**: Follow "Visual Studio defaults" with the following specific requirements:

### Brace Style
- **Allman style braces**: Each brace begins on a new line.
- **Single line statement blocks**: Can go without braces but must be properly indented on its own line and not nested in other statement blocks that use braces.
- **Exception**: A `using` statement is permitted to be nested within another `using` statement by starting on the following line at the same indentation level, even if the nested `using` contains a controlled block.

### Indentation and Spacing
- **Indentation**: Four spaces (no tabs).
- **Spurious free spaces**: Avoid, e.g., `if (someVar == 0)...` where dots mark spurious spaces.
- **Empty lines**: Avoid more than one empty line at any time between members of a type.
- **Labels**: Indent one less than the current indentation (for `goto` statements).

### Field and Property Naming
- **Internal and private fields**: Use `_camelCase` prefix with `readonly` where possible.
- **Static fields**: `readonly` should come after `static` (e.g., `static readonly` not `readonly static`).
- **Public fields**: Use PascalCasing with no prefix (use sparingly).
- **Constants**: Use PascalCasing for all constant local variables and fields (except interop code, where names and values must match the interop code exactly).
- **Fields placement**: Specify fields at the top within type declarations.

### Visibility and Modifiers
- **Always specify visibility**: Even if it's the default (e.g., `private string _foo` not `string _foo`).
- **Visibility first**: Should be the first modifier (e.g., `public abstract` not `abstract public`).
- **Modifier order**: `public`, `private`, `protected`, `internal`, `static`, `extern`, `new`, `virtual`, `abstract`, `sealed`, `override`, `readonly`, `unsafe`, `volatile`, `async`.

### Namespace and Using Statements
- **Namespace imports**: At the top of the file, outside of `namespace` declarations.
- **Sorting**: System namespaces alphabetically first, then third-party namespaces alphabetically.
- **Global using directives**: Use where appropriate to reduce repetition across files.
- **Placement**: Use `using` directives outside `namespace` declarations.

### Type Usage and Variables
- **Language keywords**: Use instead of BCL types (e.g., `int`, `string`, `float` instead of `Int32`, `String`, `Single`) for type references and method calls (e.g., `int.Parse` instead of `Int32.Parse`).
- **var usage**: Encouraged for large return types or refactoring scenarios; use full type names for clarity when needed.
- **this. avoidance**: Avoid `this.` unless absolutely necessary.
- **nameof(...)**: Use instead of string literals whenever possible and relevant.

### Code Patterns and Features
- **Method groups**: Use where appropriate.
- **Pattern matching**: Use C# 7+ pattern matching, including recursive, tuple, positional, type, relational, and list patterns for expressive conditional logic.
- **Inline out variables**: Use C# 7 inline variable feature with `out` parameters.
- **Non-ASCII characters**: Use Unicode escape sequences (`\uXXXX`) instead of literal characters to avoid garbling by tools or editors.
- **Modern C# features (C# 8–12)**:
    - Enable nullable reference types to reduce null-related errors.
    - Use ranges (`..`) and indices (`^`) for concise collection slicing.
    - Employ `using` declarations for automatic resource disposal.
    - Declare static local functions to avoid state capture.
    - Prefer switch expressions over statements for concise control flow.
    - Use records and record structs for data-centric types with value semantics.
    - Apply init-only setters for immutable properties.
    - Utilize target-typed `new` expressions to reduce verbosity.
    - Declare static anonymous functions or lambdas to prevent state capture.
    - Use file-scoped namespace declarations for concise syntax.
    - Apply `with` expressions for nondestructive mutation.
    - Use raw string literals (`"""`) for multi-line or complex strings.
    - Mark required members with the `required` modifier.
    - Use primary constructors to centralize initialization logic.
    - Employ collection expressions (`[...]`) for concise array/list/span initialization.
    - Add default parameters to lambda expressions to reduce overloads.

### Documentation Requirements
- **XML comments**: All publicly exposed methods and properties must have .NET XML comments, including protected methods of public classes.
- **Documentation culture**: Use `en-US` as specified in `src/stylecop.json`.

### File Style Precedence
- **Existing style**: If a file differs from these guidelines (e.g., private members named `m_member` instead of `_member`), the existing style in that file takes precedence.
- **Consistency**: Maintain consistency within individual files.

## Example Code Structure

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Win32;

namespace System.Collections.Generic;

public partial class ObservableLinkedList<T> : INotifyCollectionChanged, INotifyPropertyChanged
{
    private ObservableLinkedListNode<T>? _head;
    private int _count;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableLinkedList{T}"/> class.
    /// </summary>
    /// <param name="items">The items to initialize the list with.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
    public ObservableLinkedList(IEnumerable<T> items)
    {
        if (items is null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        foreach (var item in items)
        {
            AddLast(item);
        }
    }

    /// <summary>
    /// Occurs when the collection changes.
    /// </summary>
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    /// <summary>
    /// Gets the number of elements in the list.
    /// </summary>
    public int Count
    {
        get => _count;
    }

    /// <summary>
    /// Adds a new node containing the specified value at the end of the list.
    /// </summary>
    /// <param name="value">The value to add.</param>
    /// <returns>The new node that was added.</returns>
    public ObservableLinkedListNode AddLast(T value)
    {
        var newNode = new ObservableLinkedListNode(this, value);
        InsertNodeBefore(_head, newNode);
        return newNode;
    }

    /// <summary>
    /// Raises the CollectionChanged event.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }

    private void InsertNodeBefore(ObservableLinkedListNode<T>? node, ObservableLinkedListNode<T> newNode)
    {
        // Implementation details...
    }
}
```

### Notes
- **EditorConfig**: The `.editorconfig` at the root of the ReactiveUI repository enforces formatting and analysis rules, replacing the previous `analyzers.ruleset`. Update `.editorconfig` as needed to support modern C# features, such as nullable reference types.
- **Example Updates**: The example incorporates modern C# practices like file-scoped namespaces and nullable reference types. Refer to Microsoft documentation for further integration of C# 8–12 features.

### Code Formatting (Fast - Always Run)
- **ALWAYS** run formatting before committing:
  ```bash
  cd src
  dotnet format whitespace --verify-no-changes
  dotnet format style --verify-no-changes
  ```
  Time: **2-5 seconds per command**.

### Code Analysis Validation
- **Run analyzers** to check StyleCop and code quality compliance:
  ```bash
  cd src
  dotnet build --configuration Release --verbosity normal
  ```
  This runs all analyzers (StyleCop SA*, Roslynator RCS*, .NET CA*) and treats warnings as errors.
- **Analyzer Configuration**:
    - StyleCop settings in `src/stylecop.json`
    - EditorConfig rules in `.editorconfig` (root level)
    - Analyzer packages in `src/Directory.Build.props`
    - All code must follow the **ReactiveUI C# Style Guide** detailed above

### Benchmarking
- Performance testing available via BenchmarkDotNet:
  ```bash
  cd src
  dotnet run -c Release -p Benchmarks/Splat.Benchmarks.csproj
  ```
  Benchmark time: **5-15 minutes**. Set timeout to 30+ minutes.

## Key Projects and Structure

### Core Libraries (Priority Order)
1. **Splat** (`Splat.csproj`) - Main library with core functionality
2. **Splat.Builder** - AOT-friendly configuration and dependency injection builder
3. **Splat.Logging** - Cross-platform logging abstraction
4. **Splat.Drawing** - Cross-platform drawing and image handling
5. **Splat.Core** - Core interfaces and abstractions

### Dependency Injection Adapters
- **Splat.Autofac** - Autofac container adapter
- **Splat.Microsoft.Extensions.DependencyInjection** - Microsoft DI container adapter
- **Splat.SimpleInjector** - SimpleInjector container adapter
- **Splat.DryIoc** - DryIoc container adapter
- **Splat.Ninject** - Ninject container adapter

### Logging Adapters
- **Splat.Serilog** - Serilog logging adapter
- **Splat.NLog** - NLog logging adapter
- **Splat.Log4Net** - Log4Net logging adapter
- **Splat.Microsoft.Extensions.Logging** - Microsoft.Extensions.Logging adapter

### Application Performance Monitoring (APM)
- **Splat.AppCenter** - Microsoft App Center integration
- **Splat.ApplicationInsights** - Azure Application Insights integration
- **Splat.Exceptionless** - Exceptionless integration
- **Splat.Raygun** - Raygun integration

### Framework Integration
- **Splat.Prism** - Prism framework integration
- **Splat.Prism.Forms** - Prism for Xamarin.Forms integration

### Testing and Benchmarks
- **Splat.Tests** - Main test suite with comprehensive coverage
- **Splat.Aot.Tests** - AOT compatibility tests
- **Splat.*.Tests** - Individual adapter test projects
- **Benchmarks** - Performance benchmarks
- **Splat.Common.Test** - Shared test utilities

### Test Runners
- **Splat.TestRunner.Android** - Android test runner
- **Splat.TestRunner.Uwp** - UWP test runner

## Common Development Tasks

### Making Changes to Core Libraries
1. **Build individual projects** for faster iteration:
   ```bash
   dotnet build Splat/Splat.csproj --configuration Release
   ```
2. **Always** run formatting validation:
   ```bash
   dotnet format whitespace --verify-no-changes
   ```
3. **Test relevant components** after changes.

### Adding New Features
1. **Follow coding standards** - see ReactiveUI guidelines: https://www.reactiveui.net/contribute/index.html
2. **Ensure StyleCop compliance** - all code must pass StyleCop analyzers (SA* rules)
3. **Run code analysis** - `dotnet build` must complete without analyzer warnings
4. **Add unit tests** - all features require test coverage in appropriate test projects
5. **Update documentation** - especially for public APIs with XML doc comments
6. **Consider AOT compatibility** - ensure new code works with Native AOT if applicable
7. **Run benchmarks** if performance-related changes

### Working with Dependency Injection Adapters
- **Each adapter** has its own project and test project
- **Always test adapters** when making changes to core DI functionality
- **Follow adapter patterns** established in existing implementations
- **Ensure compatibility** with the target container's patterns and lifecycle

### Working with Logging Adapters
- **Test logging output** with actual logger implementations
- **Verify performance** impact of logging changes
- **Ensure structured logging** support where applicable
- **Test different log levels** and filtering scenarios

## Target Framework Support

### Supported Frameworks
- **netstandard2.0** - Broad compatibility with .NET Framework and .NET Core
- **net8.0** - Modern .NET with performance improvements
- **net9.0** - Latest .NET with newest features and AOT support

### Windows-Specific Frameworks
- **net8.0-windows10.0.17763.0** - Windows-specific APIs
- **net9.0-windows10.0.17763.0** - Latest Windows-specific APIs

### AOT Compatibility
- **net8.0 and net9.0** projects have `<IsAotCompatible>true</IsAotCompatible>`
- **Test AOT scenarios** in `Splat.Aot.Tests` project
- **Avoid reflection** where possible in AOT-compatible code paths

## Build Timing and Expectations

| Operation | Time | Notes |
|-----------|------|-------|
| **Single Project Restore** | 30 seconds | Fast operation |
| **Single Project Build** | 30-60 seconds | Usually quick |
| **Full Solution Restore** | 1-2 minutes | Many projects |
| **Full Solution Build** | 2-5 minutes | All projects and frameworks |
| **Test Suite** | 2-5 minutes | Comprehensive coverage |
| **Benchmarks** | 5-15 minutes | Performance testing |
| **Code Formatting** | 2-5 seconds | Always works |

## Performance Characteristics

### Dependency Injection Performance
- **Service resolution** should be fast (microseconds for simple registrations)
- **Container setup** one-time cost during application startup
- **Memory usage** should be minimal for service registrations

### Logging Performance
- **Structured logging** should have minimal allocation overhead
- **Log level filtering** should be efficient
- **Adapter overhead** should be minimal compared to direct logger usage

### Drawing Performance
- **Image loading** and conversion should be optimized for each platform
- **Color operations** should be lightweight
- **Geometry calculations** should avoid unnecessary allocations

## Migration and Compatibility

### Version Compatibility
- **Semantic versioning** is followed for breaking changes
- **Dependency versions** are managed via `Directory.Packages.props`
- **Target framework** support follows .NET support lifecycle

### Breaking Changes
- **Major version bumps** indicate breaking changes
- **Migration guides** provided in release notes
- **Obsolete APIs** marked with clear migration paths

## CI/CD Integration

### GitHub Actions
- Uses standard .NET GitHub Actions workflow
- Runs on multiple platforms (Windows, Linux, macOS)
- Includes code coverage reporting
- Publishes packages to NuGet

### Local Development
- **Build locally** before pushing changes
- **Run tests** for affected components
- **Format code** before every commit
- **Check analyzer warnings** before committing

## Troubleshooting

### Common Issues
1. **".NET 9.0 not supported" errors**: Install .NET 9.0 SDK
2. **StyleCop violations**: Check `.editorconfig` rules and `src/stylecop.json`
3. **Missing dependencies**: Run `dotnet restore` in `src` directory
4. **Test failures**: May be platform-specific or require specific setup

### Quick Fixes
- **Format issues**: Run `dotnet format whitespace` and `dotnet format style`
- **StyleCop violations**: Check `.editorconfig` rules and `src/stylecop.json` configuration
- **Analyzer warnings**: Build with `--verbosity normal` to see detailed analyzer messages
- **Missing XML documentation**: All public APIs require XML doc comments per StyleCop rules
- **Package restore issues**: Clear NuGet cache with `dotnet nuget locals all --clear`
- **Build configuration errors**: Ensure correct target framework for your scenario

### When to Escalate
- **Cross-platform compatibility** issues affecting core functionality
- **Performance regressions** detected in benchmarks
- **Test failures** that persist across platforms
- **Breaking changes** affecting major dependency injection containers
- **AOT compatibility** issues with newer .NET versions

## Resources

### Splat
- **Main Repository**: https://github.com/reactiveui/splat
- **Repository README**: https://github.com/reactiveui/splat#readme
- **Issues & Bug Reports**: https://github.com/reactiveui/splat/issues
- **Contributing Guidelines**: https://github.com/reactiveui/splat/blob/main/CONTRIBUTING.md
- **Code of Conduct**: https://github.com/reactiveui/splat/blob/main/CODE_OF_CONDUCT.md
- **NuGet Packages**: https://www.nuget.org/packages?q=splat
- **Main Package**: https://www.nuget.org/packages/splat
- **Code Coverage**: https://codecov.io/gh/reactiveui/splat
- **GitHub Actions (CI/CD)**: https://github.com/reactiveui/splat/actions
- **Discussions**: https://github.com/reactiveui/splat/discussions

### Governance & Contributing
- **Contribution Hub**: https://www.reactiveui.net/contribute/index.html
- **ReactiveUI Repository README**: https://github.com/reactiveui/ReactiveUI#readme
- **Contributing Guidelines**: https://github.com/reactiveui/ReactiveUI/blob/main/CONTRIBUTING.md
- **Code of Conduct**: https://github.com/reactiveui/ReactiveUI/blob/main/CODE_OF_CONDUCT.md

### Engineering & Style
- **ReactiveUI Coding/Style Guidance** (start here): https://www.reactiveui.net/contribute/
- **Build & Project Structure Reference**: https://github.com/reactiveui/ReactiveUI#readme

### Documentation & Samples
- **Documentation Home**: https://www.reactiveui.net/
- **Handbook** (core concepts): https://www.reactiveui.net/docs/
- **Official Samples Repository**: https://github.com/reactiveui/ReactiveUI.Samples

### Ecosystem
- **ReactiveUI** (MVVM framework): https://github.com/reactiveui/ReactiveUI
- **DynamicData** (reactive collections): https://github.com/reactivemarbles/DynamicData
- **Akavache** (asynchronous key-value store): https://github.com/reactiveui/Akavache

### Source Generators & AOT/Trimming
- **ReactiveUI.SourceGenerators**: https://github.com/reactiveui/ReactiveUI.SourceGenerators
- **.NET Native AOT Overview**: https://learn.microsoft.com/dotnet/core/deploying/native-aot/
- **Prepare Libraries for Trimming**: https://learn.microsoft.com/dotnet/core/deploying/trimming/prepare-libraries-for-trimming
- **Trimming Options (MSBuild)**: https://learn.microsoft.com/dotnet/core/deploying/trimming/trimming-options
- **Fixing Trim Warnings**: https://learn.microsoft.com/dotnet/core/deploying/trimming/trim-warnings

### Copilot Coding Agent
- **Best Practices for Copilot Coding Agent**: https://gh.io/copilot-coding-agent-tips

### CI & Misc
- **GitHub Actions** (builds and workflow runs): https://github.com/reactiveui/splat/actions
- **ReactiveUI Website Source** (useful for docs cross-refs): https://github.com/reactiveui/website
