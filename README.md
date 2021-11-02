## Build status

[![Build](https://github.com/retirenet/dotnet-retire/workflows/CI/badge.svg)](https://github.com/retirenet/dotnet-retire/actions)

# Components

* [![NuGet](https://img.shields.io/nuget/v/RetireNet.Runtimes.Middleware.svg)](https://www.nuget.org/packages/RetireNet.Runtimes.Middleware/)
[![NuGet](https://img.shields.io/nuget/dt/RetireNet.Runtimes.Middleware.svg)](https://www.nuget.org/packages/RetireNet.Runtimes.Middleware/) `RetireNet.Runtimes.Middleware`

* [![NuGet](https://img.shields.io/nuget/v/RetireNet.Runtimes.BackgroundServices.svg)](https://www.nuget.org/packages/RetireNet.Runtimes.BackgroundServices/)
[![NuGet](https://img.shields.io/nuget/dt/RetireNet.Runtimes.BackgroundServices.svg)](https://www.nuget.org/packages/RetireNet.Runtimes.BackgroundServices/) `RetireNet.Runtimes.BackgroundServices`



## ~~dotnet-retire~~
❗️DEPRECATED❗️

See [this issue for other solutions](https://github.com/RetireNet/dotnet-retire/issues/75).


## RetireNet.Runtimes.Middleware
We cannot detect the runtime of the app at build time, so to report use of vulnerable runtimes the app itself, the host itself can provide us reports

### Install
```
$ dotnet add package RetireNet.Runtimes.Middleware
```

### Usage

Add it to your ASP.NET Core pipeline on your preferred path:

```csharp
app.Map("/report", a => a.UseRuntimeVulnerabilityReport());
```

### What does it do?
It will fetch the releases listed in the official metadata API provided by Microsoft, and check if your app is running on a runtime with known CVEs.

Metadata endpoint used: https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/releases-index.json


### Sample output

An app running on the vulnerable 2.1.11 runtime on macOS:
```json
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

```csharp
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
