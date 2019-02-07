#load nuget:https://www.myget.org/F/reactiveui/api/v2?package=ReactiveUI.Cake.Recipe&prerelease

Environment.SetVariableNames();

// Whitelisted Packages
var packageWhitelist = new[] 
{ 
    File("./src/Splat/Splat.csproj").Path,
    File("./src/Splat.Autofac/Splat.Autofac.csproj").Path,
    File("./src/Splat.DryIoc/Splat.DryIOC.csproj").Path,
    File("./src/Splat.SimpleInjector/Splat.SimpleInjector.csproj").Path,
    File("./src/Splat.Log4Net/Splat.Log4Net.csproj").Path,
    File("./src/Splat.NLog/Splat.NLog.csproj").Path,
    File("./src/Splat.Serilog/Splat.Serilog.csproj").Path,
};

var packageTestWhitelist = new[]
{
    File("./src/Splat.Tests/Splat.Tests.csproj").Path,
    File("./src/Splat.Autofac.Tests/Splat.Autofac.Tests.csproj").Path,
    File("./src/Splat.DryIoc.Tests/Splat.DryIoc.Tests.csproj").Path,
    File("./src/Splat.SimpleInjector.Tests/Splat.SimpleInjector.Tests.csproj").Path,
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