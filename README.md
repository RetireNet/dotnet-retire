[![Build status](https://ci.appveyor.com/api/projects/status/pw65hjl5iaaouboi?svg=true)](https://ci.appveyor.com/project/JohnKorsnes/nocommonsnet)

# dotnet-retire
CLI extension to check your project for known vulnerabilities.

# Install

As the CLI don't currently allows us to install tools from the cmdline, you'll need to modify your csproj manually.
```
  <ItemGroup>
    <DotNetCliToolReference Include="dotnet-retire" Version="1.0.0" />
  </ItemGroup>
```

# Usage
```
$ dotnet restore
$ dotnet retire
```

