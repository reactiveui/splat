#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/build.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/parameters.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/credentials.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/addins.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/codecov.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/environment.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/tasks.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/testing.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/tools.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/toolsettings.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/version.cake"
#load "./src/Splat.Cake.Recipe/Cake.Recipe/Content/building.cake"

// #load nuget:?package=Splat.Cake.Recipe

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context, 
                            buildSystem: BuildSystem,
                            title: "Splat");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunDotNetCore();