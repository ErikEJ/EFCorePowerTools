# EF Core Power Tools CLI

Cross platform command line tool for advanced EF Core reverse engineering. See the full guide explaining all the features [here](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering).

## Getting started

The tool runs on any operating system with the required .NET runtime installed, and can roll forward to a newer major runtime if needed.

For a quick intro you can watch [this 2 minute demo video](https://www.youtube.com/watch?v=mtz-O6VXAc0&t=56s).

And there is a longer [30 minute demo here](https://www.youtube.com/watch?v=fwR59ep-2-8).

### Installing the tool

EF Core 10:

```bash
dotnet tool install ErikEJ.EFCorePowerTools.Cli -g --version 10.*
```

EF Core 8:

```bash
dotnet tool install ErikEJ.EFCorePowerTools.Cli -g --version 8.*
```

EF Core 9:

```bash
dotnet tool install ErikEJ.EFCorePowerTools.Cli -g --version 9.*
```

### Running the tool 

From the folder where you want the code to be generated (usually where your .NET project is located)

```bash
efcpt "Server=(local);Initial Catalog=Northwind;User id=user;Pwd=secret123;Encrypt=false" mssql
```

Type `efcpt --help` for help on command line options.

The provider name (`mssql`) may not be required, as an attempt is made to resolve the provider from the connection string.

### Using a .dacpac file

Instead of a connection string, you can use the path to a SQL Server .dacpac file. The provider is resolved automatically.

```bash
efcpt "Database/bin/Debug/net10.0/Database.dacpac"
```

You can build the .dacpac with `dotnet build` from an SDK-style database project, for example [MSBuild.Sdk.SqlProj](https://github.com/rr-wfm/MSBuild.Sdk.SqlProj) or [Microsoft.Build.Sql](https://github.com/microsoft/DacFx). Then no SQL Server is required, neither to build the .dacpac nor to generate the code.

### Configuring options

A configuration file `efcpt-config.json` is created in the output folder, and you can open this file in your editor to modify the default options. If your editor supports it (for example VS Code), it will provide syntax guidance for the file. For reference there is a fully populated sample file [here](https://github.com/ErikEJ/EFCorePowerTools/blob/master/samples/efcpt-config.json).

The [Reverse Engineering guide](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering) describes the options. For each option, it shows the key in `efcpt-config.json` as `CLI: section/key`, for example `CLI: code-generation/use-t4`.

### Updating to new configuration

After updating the `efcpt-config.json`, you will need to run the `efcpt` CLI command from above once again in order to update the generated code.

If you have updated the configuration file in a way that requires files to be deleted - by excluding objects for example - then you will need to set the `"soft-delete-obsolete-files"` option in the configuration file to `true` or delete the files manually.

### Excluding objects

The config file defaults to always contain all current database objects. 

If you don't want the lists of objects to be refreshed during each scaffolding operation, set the `"refresh-object-lists"` option in the configuration file to `false`.

You can exclude individual database objects with `"exclude": true` for the object.

You can also use the `exclusionWildcard` item under each type of data object to filter included objects. 

You can use the following filter expressions:

- `*`: Exclude all objects in section. Overrides all other filters.
- `abc*`: Exclude all objects in section that *starts* with `abc`. Multiple filters allowed.
- `*xyz`: Exclude all objects in section that *ends* with `xyz`. Multiple filters allowed.
- `*mno*`: Exclude all objects in section that *contains* `mno`. Multiple filters allowed.

Filters will apply unless `"exclude": false` is explicitly set for a database object.

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
         "name": "[dbo].[Messages]"
      }
  ],
```

In the example above, only the Users table will be selected.

```json
"tables": [
      {
         "exclusionWildcard": "[other].*"
      },
      {
         "name": "[dbo].[Users]"
      },
      {
         "name": "[other].[Accounts]"
      },      
      {
         "name": "[other].[Messages]",
         "exclude": false
      }
  ],
```

In the example above, Users and Messages tables will be selected.

```json
"tables": [
      {
         "exclusionWildcard": "[other].*"
      },
      {
         "exclusionWildcard": "[other2].*"
      },
      {
         "name": "[dbo].[Users]"
      },
      {
         "name": "[other].[Accounts]"
      },      
      {
         "name": "[other].[Messages]"
      },
      {
         "name": "[other2].[Actions]"
      }
  ],
```

In the example above, Users table will be selected.


### Generate a Mermaid ER diagram

The tool can generate a [Mermaid ER diagram](https://mermaid.js.org/syntax/entityRelationshipDiagram.html) during execution, just set the `code-generation` option `generate-mermaid-diagram` to `true` and a `dbdiagram.md` file will be created in the output folder.

### Line endings and character set

By default, the generated files use the line endings of the operating system where the tool runs, and are UTF-8 with a byte order mark (BOM). To get the same files on every operating system, for example to match your `.editorconfig`, set the `code-generation` options `file-line-endings` and `file-charset`:

```json
"code-generation": {
   "file-charset": "utf-8",
   "file-line-endings": "crlf"
}
```

- `file-line-endings`: `native` (default), `crlf` or `lf`
- `file-charset`: `utf-8-bom` (default) or `utf-8` (without a BOM). These are the same values as the `.editorconfig` `charset` property.

The options apply to the generated code files, `efcpt-readme.md` and `dbdiagram.md`.

### Updating the tool

```bash
dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 10.*
```

[Release notes](https://github.com/ErikEJ/EFCorePowerTools/wiki/Release-notes) - notice the `+CLI` label.

### Getting the latest daily build

```bash
dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 10.*-*
```
