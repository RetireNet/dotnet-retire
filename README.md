[![Build status](https://ci.appveyor.com/api/projects/status/6y4yrtkhofgcswqt?svg=true)](https://ci.appveyor.com/project/JohnKorsnes/dotnet-retire) [![NuGet](https://img.shields.io/nuget/v/dotnet-retire.svg)](https://www.nuget.org/packages/dotnet-retire/)
[![NuGet](https://img.shields.io/nuget/dt/dotnet-retire.svg)](https://www.nuget.org/packages/dotnet-retire/)
## dotnet-retire
A `dotnet` CLI extension to check your project for known vulnerabilities.

## Install
As the CLI don't currently allows us to install tools from the cmdline, you'll need to modify your csproj manually.
```
  <ItemGroup>
    <DotNetCliToolReference Include="dotnet-retire" Version="1.0.1" />
  </ItemGroup>
```
Or if your project is still using the preview2 tooling, modify your `project.json`
```
  "tools": {
    "dotnet-retire": "1.0.1"
  }
```

## Usage
```
$ dotnet retire
```

### Sample output:
![image](https://cloud.githubusercontent.com/assets/206726/26074074/d5bc2ee4-39b0-11e7-9018-08dd305b96a9.png)

# How does it work?
It fetches the packages listed in the corresponding `packages` repo in this GitHub organization ([link](https://github.com/RetireNet/Packages/blob/master/Content/1.json)), and checks your projects `obj\project.assets.json` or `project.lock.json`  file for any match (direct, or transient). 

Keeping the list of packages up to date will be done via updating that repo when announcements occur from Microsoft with additional json files with links to announcements from Microsofts security team.
