#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

Task("Build")
    .Does(() =>
    {
        var solutionFile = "./SimpleValueObjects.sln";

        MSBuild(solutionFile, settings => settings
            .SetConfiguration("Release")
            .SetVerbosity(Verbosity.Minimal)
            .WithTarget("Clean")
        );

        NuGetRestore(solutionFile);
        DotNetCoreRestore(".");

        MSBuild(solutionFile, settings => settings
            .SetConfiguration("Release")
            .SetVerbosity(Verbosity.Minimal)
        );
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        NUnit3("./SimpleValueObjects.Tests/bin/Release/SimpleValueObjects.Tests.dll", new NUnit3Settings
        {
            NoResults = true
        });
    });

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
    {
        DotNetCorePack("./SimpleValueObjects/SimpleValueObjects.csproj");
    });

Task("Default")
    .IsDependentOn("Test")
    .IsDependentOn("Pack");

RunTarget(Argument("target", "Default"));