## Build status

| Build server                | Platform     | Status                                                                                                                    |
|-----------------------------|--------------|---------------------------------------------------------------------------------------------------------------------------|
| AppVeyor                    | Windows      | [![Build status](https://ci.appveyor.com/api/projects/status/6y4yrtkhofgcswqt/branch/master?svg=true)](https://ci.appveyor.com/project/JohnKorsnes/dotnet-retire/branch/master)|
| Travis                      | Linux        | [![Build Status](https://travis-ci.org/RetireNet/dotnet-retire.svg?branch=master)](https://travis-ci.org/RetireNet/dotnet-retire)|
| Azure DevOps | Linux | [![Build Status](https://dev.azure.com/RetireNET/dotnet-retire/_apis/build/status/RetireNet.dotnet-retire?branchName=master)](https://dev.azure.com/RetireNET/dotnet-retire/_build/latest?definitionId=1)|



# Components

* [![NuGet](https://img.shields.io/nuget/v/dotnet-retire.svg)](https://www.nuget.org/packages/dotnet-retire/)
[![NuGet](https://img.shields.io/nuget/dt/dotnet-retire.svg)](https://www.nuget.org/packages/dotnet-retire/) `dotnet-retire`

* [![NuGet](https://img.shields.io/nuget/v/RetireNet.Runtimes.Middleware.svg)](https://www.nuget.org/packages/RetireNet.Runtimes.Middleware/)
[![NuGet](https://img.shields.io/nuget/dt/RetireNet.Runtimes.Middleware.svg)](https://www.nuget.org/packages/RetireNet.Runtimes.Middleware/) `RetireNet.Runtimes.Middleware`

* [![NuGet](https://img.shields.io/nuget/v/RetireNet.Runtimes.BackgroundServices.svg)](https://www.nuget.org/packages/RetireNet.Runtimes.BackgroundServices/)
[![NuGet](https://img.shields.io/nuget/dt/RetireNet.Runtimes.BackgroundServices.svg)](https://www.nuget.org/packages/RetireNet.Runtimes.BackgroundServices/) `RetireNet.Runtimes.BackgroundServices`



## dotnet-retire

A `dotnet` CLI extension to check your project for known vulnerabilities.


### Install
```
$ dotnet tool install -g dotnet-retire
```

### Usage
```
$ dotnet retire
```

Additional options:

  - `loglevel=Trace|Debug|Information|Warning|Error|Critical` (default:`Information`)

  - `rooturl=<url>` to feed> (default:[https://raw.githubusercontent.com/RetireNet/Packages/master/index.json](https://raw.githubusercontent.com/RetireNet/Packages/master/index.json))

Sample:

```
$ dotnet retire loglevel=debug
```

#### Sample output:
![image](https://user-images.githubusercontent.com/206726/26968418-3c4c6296-4d02-11e7-9cf9-754533c1a594.png)

### How does it work?
It fetches the packages listed in the corresponding `packages` repo in this GitHub organization ([link](https://github.com/RetireNet/Packages/blob/master/Content/1.json)), and checks your projects `obj\project.assets.json` or `project.lock.json`  file for any match (direct, or transient).

Keeping the list of packages up to date will be done via updating that repo when announcements occur from Microsoft with additional json files with links to announcements from Microsofts security team.

### Other projects with similar functionality:
#### [SafeNuGet](https://github.com/owasp/safenuget)
Runs as part of the build (MSBuild target). Analyzes packages.config, does not handle transient dependencies.
#### [DevAudit](https://github.com/OSSIndex/DevAudit)
Standalone .NET console app that analyzes a packages.config. Analyzes packages.config, does not handle transient dependencies.


## RetireNet.Runtimes.Middleware
We cannot detect the runtime of the app at build time, so to report use of vulnerable runtimes the app itself, the host itself can provide us reports

### Install
```
$ dotnet add package RetireNet.Runtimes.Middleware
```

### Usage

Add it to your ASP.NET Core pipeline on your preferred path:

```
app.Map("/report", a => a.UseRuntimeVulnerabilityReport());
```

### What does it do?
It will fetch the releases listed in the official metadata API provided by Microsoft, and check if your app is running on a runtime with known CVEs.

Metadata endpoint used: https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/releases-index.json


### Sample output

An app running on the vulnerable 2.1.11 runtime on macOS:
```
{
    "isVulnerable": true,
    "appRuntimeDetails": {
        "os": "OSX",
        "osPlatform": "Darwin 18.6.0 Darwin Kernel Version 18.6.0: Thu Apr 25 23:16:27 PDT 2019; root:xnu-4903.261.4~2/RELEASE_X86_64",
        "osArchitecture": "X64",
        "osBits": "64",
        "appTargetFramework": ".NETCoreApp,Version=v2.1",
        "appRuntimeVersion": "2.1.11",
        "appBits": "64"
    },
    "securityRelease": {
        "runtimeVersion": "2.1.13",
        "cvEs": [
            {
                "cve-id": " CVE-2018-8269",
                "cve-url": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2018-8269"
            },
            {
                "cve-id": " CVE-2019-1301",
                "cve-url": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2019-1301"
            },
            {
                "cve-id": " CVE-2019-1302",
                "cve-url": "https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2019-1302"
            }
        ]
    }
}
```

## RetireNet.Runtimes.BackgroundServices
This is the same report as for the middleware, only logging it using the configured `ILogger` as a _WARN_ log statment.

### Install
```
$ dotnet add package RetireNet.Runtimes.BackgroundServices
```

### Usage

Register it into the container, and provide it a interval in milliseconds how often you would like the check to execute.

```
services.AddRetireRuntimeHostedService(c => c.CheckInterval = 60000)
```

### What does it do?
The same as for the middleware endpoint.


### Sample output

An app running on the vulnerable 2.1.11 runtime on macOS, using the `ConsoleLogger`:
```
warn: RetireNet.Runtimes.BackgroundServices.RetireRuntimeBackgroundService[0]
      Running on vulnerable runtime 2.1.11. Security release 2.1.13
```
