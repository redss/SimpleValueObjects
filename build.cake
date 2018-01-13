var configuration = "Release";

Task("Build")
    .Does(() =>
    {
        var solutionPath = ".";

        DotNetCoreClean(solutionPath, new DotNetCoreCleanSettings
        {
            Configuration = configuration
        });

        DotNetCoreBuild(solutionPath, new DotNetCoreBuildSettings
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
        DotNetCorePack("./SimpleValueObjects", new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = "./Artifacts"
        });
    });

Task("Default")
    .IsDependentOn("Test")
    .IsDependentOn("Pack");

RunTarget(Argument("target", "Default"));