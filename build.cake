var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");

var dotnetRetireProjName = "dotnet-retire";
var dotnetRetireProj= $"./{dotnetRetireProjName}/{dotnetRetireProjName}.csproj";
var dotnetRetireVersion = "2.3.2";

var dotnetMiddlewareName = "RetireRuntimeMiddleware";
var dotnetMiddlewareProj= $"./{dotnetMiddlewareName}/{dotnetMiddlewareName}.csproj";
var dotnetMiddlewareVersion = "0.2.1";

var outputDir = "./output";

var sln = "dotnet-retire.sln";

Task("Build")
    .Does(() => {
        DotNetCoreBuild(sln, new DotNetCoreBuildSettings { Configuration = "Release" });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCoreTest($"./Tests/Tests.csproj");
});

Task("Pack")
    .IsDependentOn("Test")
    .Does(() => {
        PackIt(dotnetRetireProj, dotnetRetireVersion);
        PackIt(dotnetMiddlewareName, dotnetMiddlewareVersion);
});

private void PackIt(string project, string version)
{
    var coresettings = new DotNetCorePackSettings
    {
        Configuration = "Release",
        OutputDirectory = outputDir,
        NoBuild = true
    };
    coresettings.MSBuildSettings = new DotNetCoreMSBuildSettings()
                                    .WithProperty("Version", new[] { version });


    DotNetCorePack(project, coresettings);
}

Task("PublishDotnetRetire")
    .IsDependentOn("Pack")
    .Does(() => {
        var settings = new DotNetCoreNuGetPushSettings
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = EnvironmentVariable("NUGET_API_KEY")
        };

        DotNetCoreNuGetPush($"{outputDir}/{dotnetRetireProjName}.{dotnetRetireVersion}.nupkg", settings);
});

Task("PublishMiddleware")
    .IsDependentOn("Pack")
    .Does(() => {
        var settings = new DotNetCoreNuGetPushSettings
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = EnvironmentVariable("NUGET_API_KEY")
        };
        DotNetCoreNuGetPush($"{outputDir}/{dotnetMiddlewareName}.{dotnetMiddlewareVersion}.nupkg", settings);
});

RunTarget(target);
