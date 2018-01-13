var configuration = "Release";

Task("Build")
    .Does(() =>
    {
        var solutionFile = "./SimpleValueObjects.sln";

        DotNetCoreClean(solutionFile, new DotNetCoreCleanSettings
        {
            Configuration = configuration
        });

        DotNetCoreRestore(".");

        DotNetCoreBuild(solutionFile, new DotNetCoreBuildSettings
        {
            Configuration = configuration
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        DotNetCoreTest("./SimpleValueObjects.Tests", new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoBuild = true
        });
    });

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
    {
        DotNetCorePack("./SimpleValueObjects/SimpleValueObjects.csproj", new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = "./Artifacts"
        });
    });

Task("Default")
    .IsDependentOn("Test")
    .IsDependentOn("Pack");

RunTarget(Argument("target", "Default"));