var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");
var proj = $"./dotnet-retire/dotnet-retire.csproj";

var version = "2.1.0";
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

Task("PublishToNugetOrg")
    .IsDependentOn("Pack")
    .Does(() => {
        var settings = new DotNetCoreNuGetPushSettings
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = Argument("nugetapikey", "must-be-given")
        };

        DotNetCoreNuGetPush($"{outputDir}/PayEx.Client.{version}.nupkg", settings);
});

RunTarget(target);
