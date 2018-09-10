## Build status

| Build server                | Platform     | Status                                                                                                                    |
|-----------------------------|--------------|---------------------------------------------------------------------------------------------------------------------------|
| AppVeyor                    | Windows      | [![Build status](https://ci.appveyor.com/api/projects/status/6y4yrtkhofgcswqt/branch/master?svg=true)](https://ci.appveyor.com/project/JohnKorsnes/dotnet-retire/branch/master)|
| Travis                      | Linux        | [![Build Status](https://travis-ci.org/RetireNet/dotnet-retire.svg?branch=master)](https://travis-ci.org/RetireNet/dotnet-retire)|
| Azure DevOps | Linux | [![Build Status](https://dev.azure.com/RetireNET/dotnet-retire/_apis/build/status/RetireNet.dotnet-retire?branchName=master)](https://dev.azure.com/RetireNET/dotnet-retire/_build/latest?definitionId=1)|


 [![NuGet](https://img.shields.io/nuget/v/dotnet-retire.svg)](https://www.nuget.org/packages/dotnet-retire/)
[![NuGet](https://img.shields.io/nuget/dt/dotnet-retire.svg)](https://www.nuget.org/packages/dotnet-retire/)
## dotnet-retire
A `dotnet` CLI extension to check your project for known vulnerabilities.

## Install
```
$ dotnet tool install -g dotnet-retire
```

## Usage
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

### Sample output:
![image](https://user-images.githubusercontent.com/206726/26968418-3c4c6296-4d02-11e7-9cf9-754533c1a594.png)

# How does it work?
It fetches the packages listed in the corresponding `packages` repo in this GitHub organization ([link](https://github.com/RetireNet/Packages/blob/master/Content/1.json)), and checks your projects `obj\project.assets.json` or `project.lock.json`  file for any match (direct, or transient).

Keeping the list of packages up to date will be done via updating that repo when announcements occur from Microsoft with additional json files with links to announcements from Microsofts security team.

## Other projects with similar functionality:
### [SafeNuGet](https://github.com/owasp/safenuget)
Runs as part of the build (MSBuild target). Analyzes packages.config, does not handle transient dependencies.
### [DevAudit](https://github.com/OSSIndex/DevAudit)
Standalone .NET console app that analyzes a packages.config. Analyzes packages.config, does not handle transient dependencies.
