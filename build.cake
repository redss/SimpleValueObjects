#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

var target = Argument("target", "Default");

var solutionFile = "./SimpleValueObjects.sln";

Task("Build")
    .Does(() =>
    {
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

// todo: create package

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);
