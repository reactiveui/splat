#load nuget:https://pkgs.dev.azure.com/dotnet/ReactiveUI/_packaging/ReactiveUI/nuget/v3/index.json?package=ReactiveUI.Cake.Recipe&prerelease

Environment.SetVariableNames();

// Whitelisted Packages
var packageWhitelist = new[] 
{ 
    MakeAbsolute(File("./src/Splat/Splat.csproj")),
    MakeAbsolute(File("./src/Splat.Autofac/Splat.Autofac.csproj")),
    MakeAbsolute(File("./src/Splat.DryIoc/Splat.DryIoc.csproj")),
    MakeAbsolute(File("./src/Splat.Log4Net/Splat.Log4Net.csproj")),
    MakeAbsolute(File("./src/Splat.Microsoft.Extensions.DependencyInjection/Splat.Microsoft.Extensions.DependencyInjection.csproj")),
    MakeAbsolute(File("./src/Splat.Microsoft.Extensions.Logging/Splat.Microsoft.Extensions.Logging.csproj")),
    MakeAbsolute(File("./src/Splat.Ninject/Splat.Ninject.csproj")),
    MakeAbsolute(File("./src/Splat.NLog/Splat.NLog.csproj")),
    MakeAbsolute(File("./src/Splat.Serilog/Splat.Serilog.csproj")),
    MakeAbsolute(File("./src/Splat.SimpleInjector/Splat.SimpleInjector.csproj")),
    MakeAbsolute(File("./src/Splat.Drawing/Splat.Drawing.csproj")),
};

var packageTestWhitelist = new[]
{
    MakeAbsolute(File("./src/Splat.Tests/Splat.Tests.csproj")),
    MakeAbsolute(File("./src/Splat.Autofac.Tests/Splat.Autofac.Tests.csproj")),
    MakeAbsolute(File("./src/Splat.DryIoc.Tests/Splat.DryIoc.Tests.csproj")),
    MakeAbsolute(File("./src/Splat.Microsoft.Extensions.DependencyInjection.Tests/Splat.Microsoft.Extensions.DependencyInjection.Tests.csproj")),
    MakeAbsolute(File("./src/Splat.Ninject.Tests/Splat.Ninject.Tests.csproj")),
    MakeAbsolute(File("./src/Splat.SimpleInjector.Tests/Splat.SimpleInjector.Tests.csproj")),
};

BuildParameters.SetParameters(context: Context, 
                            buildSystem: BuildSystem,
                            title: "Splat",
                            whitelistPackages: packageWhitelist,
                            whitelistTestPackages: packageTestWhitelist,
                            artifactsDirectory: "./artifacts",
                            sourceDirectory: "./src");

ToolSettings.SetToolSettings(context: Context, usePrereleaseMsBuild: true);

Build.Run();
