# EF Core Power Tools CLI

Cross platform command line tool for advanced EF Core reverse engineering. See the full guide explaining all the features [here](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering).

## Getting started

The tool runs on any operating system with the .NET 6.0 runtime installed.

### Installing the tool

For EF Core 7:

```bash
dotnet tool install ErikEJ.EFCorePowerTools.Cli -g --version 7.0.*-*
```

For EF Core 6:

```bash
dotnet tool install ErikEJ.EFCorePowerTools.Cli -g --version 6.0.*-*
```

### Running the tool 

From the folder where you want the code to be generated (usually where your .NET project is located)

```bash
efcpt "Server=(local);Database=Northwind;User id=user;Pwd=secret123;Encrypt=false" mssql
```

### Configuring options

A configuration file `efcpt-config.json` is created in the output folder, and you can open this file in your editor to modify the default options. If your editor supports it (for example VS Code), it will provide syntax guidance for the file. For reference there is a fully populated sample file [here](https://github.com/ErikEJ/EFCorePowerTools/blob/master/samples/efcpt-config.json).

### Updating to new configuration

After updating the `efcpt-config.json`, you will need to run the `efcpt` CLI command from above once again in order to update the generated code.

If you have updated the configuration file in a way that requires files to be deleted - by excluding objects for example - then you will need to set the `"soft-delete-obsolete-files"` option in the configuration file to `true` or delete the files manually.

### Excluding objects

The config file will always contain all current database objects. You can use the `exclusionWildcard` item under each type of data object to filter included objects. 

You can use the following filter expressions:

- `abc*`: Starts with. NOTE: Not implemented yet. 
- `*xyz`: Ends with. NOTE: Not implemented yet.
- `*`: Exclude all objects in section unless `"exclude": false` is explicitly set.

All filters are case sensitive.

```json
"tables": [
      {
         "exclusionWildcard": "*"
      },
      {
         "name": "[dbo].[Users]",
         "exclude": false
      },
      {
         "name": "[dbo].[Accounts]",
         "exclude": false
      },
      {
         "name": "[dbo].[Messages]"
      },
      {
         "name": "[dbo].[Payments]"
      }
  ],
```

In the example above, only Users and Accounts tables will be selected.

### Updating the tool

```bash
dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 7.0.*-*
```
