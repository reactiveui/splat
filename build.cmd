@echo off
tools\nuget\nuget.exe update -self -verbosity quiet
tools\nuget\nuget.exe install xunit.runner.console -OutputDirectory tools -ExcludeVersion -verbosity quiet
tools\nuget\nuget.exe install Cake -OutputDirectory tools -ExcludeVersion -verbosity quiet

tools\Cake\Cake.exe build.cake --target=%1

exit /b %errorlevel%
