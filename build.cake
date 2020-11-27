var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");

var dotnetRetireProjName = "RetireNet.Packages.Tool";
var dotnetRetirePackageId = "dotnet-retire";
var dotnetRetireProj= $"./src/{dotnetRetireProjName}/{dotnetRetireProjName}.csproj";
var dotnetRetireVersion = "5.0.0";

var dotnetMiddlewareName = "RetireNet.Runtimes.Middleware";
var dotnetMiddlewarePackageId = "RetireNet.Runtimes.Middleware";
var dotnetMiddlewareProj= $"./src/{dotnetMiddlewareName}/{dotnetMiddlewareName}.csproj";

var dotnetBackgroundServiceName = "RetireNet.Runtimes.BackgroundServices";
var dotnetBackgroundServicePackageId = "RetireNet.Runtimes.BackgroundServices";
var dotnetBackgroundServiceProj = $"./src/{dotnetBackgroundServiceName}/{dotnetBackgroundServiceName}.csproj";

var runtimeCheckersVersion = "5.0.0";

var outputDir = "./output";

var sln = "dotnet-retire.sln";

Task("Build")
    .Does(() => {
        DotNetCoreBuild(sln, new DotNetCoreBuildSettings {
            Configuration = "Release" });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        var projectFiles = GetFiles("./test/**/*.csproj");
        foreach(var file in projectFiles)
        {
            DotNetCoreTest(file.FullPath, new DotNetCoreTestSettings {
                ArgumentCustomization = args=>args.Append("/p:CollectCoverage=true /p:CoverletOutputFormat=opencover")
            });
        }
});

Task("Pack")
    .IsDependentOn("Test")
    .Does(() => {
        PackIt(dotnetRetireProj, dotnetRetireVersion);
        PackIt(dotnetMiddlewareProj, runtimeCheckersVersion);
        PackIt(dotnetBackgroundServiceProj, runtimeCheckersVersion);
});

private void PackIt(string project, string version, string tfm = null)
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

Task("PublishTool")
    .IsDependentOn("Pack")
    .Does(() => {
        var settings = new DotNetCoreNuGetPushSettings
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = EnvironmentVariable("NUGET_API_KEY")
        };

        DotNetCoreNuGetPush($"{outputDir}/{dotnetRetirePackageId}.{dotnetRetireVersion}.nupkg", settings);
});

Task("PublishRuntimeCheckers")
    .IsDependentOn("Pack")
    .Does(() => {
        var settings = new DotNetCoreNuGetPushSettings
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = EnvironmentVariable("NUGET_API_KEY")
        };
        DotNetCoreNuGetPush($"{outputDir}/{dotnetMiddlewarePackageId}.{runtimeCheckersVersion}.nupkg", settings);
        DotNetCoreNuGetPush($"{outputDir}/{dotnetBackgroundServicePackageId}.{runtimeCheckersVersion}.nupkg", settings);
});

RunTarget(target);
