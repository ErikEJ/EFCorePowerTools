# EF Core Power Tools CLI

Cross platform command line tool for advanced EF Core reverse engineering. See the full guide explaining all the features [here](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering).

## Getting started

The tool runs on any computer with the .NET 6.0 runtime installed.

### Install the tool

For EF Core 7:

```bash
dotnet tool install ErikEJ.EFCorePowerTools.Cli -g --version 7.0.*-*
```

For EF Core 6:

```bash
dotnet tool install ErikEJ.EFCorePowerTools.Cli -g --version 6.0.*-*
```

### Run the tool 

From the folder where you want the code to be generated (usually where your .NET project is located)

```bash
efcpt "Server=(local);Database=Northwind;User id=user;Pwd=secret123;Encrypt=false" mssql
```

### Configure options

A configuration file `efcpt-config.json` is created in the output folder, and you can open this file in your editor to modify the default options. If your editor supports it (for example VS Code), it will provide syntax guidance for the file.

### Update the tool

```bash
dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 7.0.*-*
```
