#load nuget:https://www.myget.org/F/reactiveui/api/v2?package=ReactiveUI.Cake.Recipe&prerelease

Environment.SetVariableNames();

// Whitelisted Packages
var packageWhitelist = new[] 
{ 
    MakeAbsolute(File("./src/Splat/Splat.csproj")),
    MakeAbsolute(File("./src/Splat.Autofac/Splat.Autofac.csproj")),
    MakeAbsolute(File("./src/Splat.DryIoc/Splat.DryIoc.csproj")),
    MakeAbsolute(File("./src/Splat.Ninject/Splat.Ninject.csproj")),
    MakeAbsolute(File("./src/Splat.SimpleInjector/Splat.SimpleInjector.csproj")),
    MakeAbsolute(File("./src/Splat.Log4Net/Splat.Log4Net.csproj")),
    MakeAbsolute(File("./src/Splat.NLog/Splat.NLog.csproj")),
    MakeAbsolute(File("./src/Splat.Serilog/Splat.Serilog.csproj")),
    MakeAbsolute(File("./src/Splat.Microsoft.Extensions.Logging/Splat.Microsoft.Extensions.Logging.csproj")),
};

var packageTestWhitelist = new[]
{
    MakeAbsolute(File("./src/Splat.Tests/Splat.Tests.csproj")),
    MakeAbsolute(File("./src/Splat.Autofac.Tests/Splat.Autofac.Tests.csproj")),
    MakeAbsolute(File("./src/Splat.DryIoc.Tests/Splat.DryIoc.Tests.csproj")),
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

ToolSettings.SetToolSettings(context: Context);

Build.Run();
