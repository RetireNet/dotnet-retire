var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");
var projName = "dotnet-retire";
var proj = $"./{projName}/{projName}.csproj";

var version = "2.3.2";
var outputDir = "./output";

Task("Build")
    .Does(() => {
        DotNetCoreBuild(proj, new DotNetCoreBuildSettings { Configuration = "Release" });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        var testproj = $"./Tests/Tests.csproj";
        DotNetCoreTest(testproj);
});

Task("Pack")
    .IsDependentOn("Test")
    .Does(() => {
        var coresettings = new DotNetCorePackSettings
        {
            Configuration = "Release",
            OutputDirectory = outputDir,
        };
        coresettings.MSBuildSettings = new DotNetCoreMSBuildSettings()
                                        .WithProperty("Version", new[] { version });


        DotNetCorePack(proj, coresettings);
});

Task("Publish")
    .IsDependentOn("Pack")
    .Does(() => {
        var settings = new DotNetCoreNuGetPushSettings
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = EnvironmentVariable("NUGET_API_KEY")
        };

        DotNetCoreNuGetPush($"{outputDir}/{projName}.{version}.nupkg", settings);
});

RunTarget(target);
