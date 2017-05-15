[![Build status](https://ci.appveyor.com/api/projects/status/6y4yrtkhofgcswqt?svg=true)](https://ci.appveyor.com/project/JohnKorsnes/dotnet-retire)
[![NuGet](https://img.shields.io/nuget/dt/dotnet-retire.svg)]()
## dotnet-retire
A `dotnet` CLI extension to check your project for known vulnerabilities.

## Install
As the CLI don't currently allows us to install tools from the cmdline, you'll need to modify your csproj manually.
```
  <ItemGroup>
    <DotNetCliToolReference Include="dotnet-retire" Version="1.0.0" />
  </ItemGroup>
```

## Usage
```
$ dotnet retire
```

### Sample output:

```

