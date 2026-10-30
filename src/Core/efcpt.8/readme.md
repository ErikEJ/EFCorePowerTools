# EF Core Power Tools CLI

Cross platform command line tool for advanced EF Core reverse engineering. See the full guide explaining all the features [here](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering).

[![CLI build](https://github.com/ErikEJ/EFCorePowerTools/actions/workflows/cli-tool.yml/badge.svg?branch=master)](https://github.com/ErikEJ/EFCorePowerTools/actions/workflows/cli-tool.yml?query=branch%3Amaster)
[![CLI version including nightly builds](https://img.shields.io/nuget/vpre/ErikEJ.EFCorePowerTools.Cli?label=CLI%20%28incl.%20nightly%29)](https://github.com/ErikEJ/EFCorePowerTools/blob/master/src/Core/efcpt.8/readme.md#getting-the-latest-daily-build)

## Getting started

The tool runs on any operating system with the required .NET runtime installed, and can roll forward to a newer major runtime if needed.

For a quick intro you can watch [this 2 minute demo video](https://www.youtube.com/watch?v=mtz-O6VXAc0&t=56s).

And there is a longer [30 minute demo here](https://www.youtube.com/watch?v=fwR59ep-2-8).

### Installing the tool

The CLI is published on NuGet as [ErikEJ.EFCorePowerTools.Cli](https://www.nuget.org/packages/ErikEJ.EFCorePowerTools.Cli). Install it as a .NET tool using the command for your EF Core version:

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

The following options control the CLI input and output:

| Option | Purpose | Default |
| --- | --- | --- |
| `-o`, `--output` | Root folder for generated files. | Current directory |
| `-i`, `--input` | Path to the CLI configuration file to read or create. | `efcpt-config.json` in the current directory |
| `-r`, `--renamer` | Path to a [custom renaming file](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering#custom-renaming-with-efptrenamingjson). | `efpt.renaming.json` in the current directory |
| `-v`, `--verbose` | Show more detailed output, including generated file names. | Disabled |

`--output` does not change the default configuration or renaming file locations. For example, to keep the configuration and generated code in a project subfolder, specify both paths:

```bash
efcpt "Database/bin/Debug/net10.0/Database.dacpac" --input "src/Data/efcpt-config.json" --output "src/Data"
```

### Using a .dacpac file

Instead of a connection string, you can use the path to a SQL Server .dacpac file. The provider is resolved automatically.

```bash
efcpt "Database/bin/Debug/net10.0/Database.dacpac"
```

You can build the .dacpac with `dotnet build` from an SDK-style database project, for example [MSBuild.Sdk.SqlProj](https://github.com/rr-wfm/MSBuild.Sdk.SqlProj) or [Microsoft.Build.Sql](https://github.com/microsoft/DacFx). Then no SQL Server is required, neither to build the .dacpac nor to generate the code.

For including referenced .dacpac files, see [the guide on the wiki](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering#including-referenced-dacpac-files).

### Configuring options

A configuration file `efcpt-config.json` is created in the current directory, or at the path specified by `--input`. Open this file in your editor to modify the default options. If your editor supports it (for example VS Code), it will provide syntax guidance using the file's `$schema` reference. See the [sample configuration](https://github.com/ErikEJ/EFCorePowerTools/blob/master/samples/efcpt-config.json) and [configuration schema](https://github.com/ErikEJ/EFCorePowerTools/blob/master/samples/efcpt-config.schema.json) for the available keys and value types.

The [Reverse Engineering guide](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering) describes the options. Its `CLI: section/key` tags identify the corresponding setting in `efcpt-config.json`. For example, `CLI: code-generation/use-t4` means the `use-t4` property inside the `code-generation` object. Examples using `efpt.config.json` refer to the Visual Studio configuration format; use the tagged CLI keys in `efcpt-config.json`.

### Updating to new configuration

After updating the `efcpt-config.json`, you will need to run the `efcpt` CLI command from above once again in order to update the generated code.

If you have updated the configuration file in a way that requires files to be deleted - by excluding objects for example - then you will need to set the `"soft-delete-obsolete-files"` option in the configuration file to `true` or delete the files manually.

### Excluding objects

By default, `code-generation/refresh-object-lists` is `true`. Each run updates the `tables`, `views`, `stored-procedures` and `functions` lists with the current database objects, retaining settings for existing objects. Newly discovered objects are included unless an exclusion rule applies.

To keep a fixed selection, set `refresh-object-lists` to `false` in `code-generation`. Only the objects listed in the configuration are then considered for generation; add new objects to the lists manually or re-enable refreshing to discover them. When the configuration file already exists, this also prevents the CLI from rewriting it, preserving your formatting and manual grouping of objects.

You can exclude individual database objects with `"exclude": true` for the object.

You can also add an `exclusionWildcard` item to each of the `tables`, `views`, `stored-procedures` and `functions` arrays to filter included objects. Patterns match the full object name as it appears in the configuration, including the schema where present.

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

#### Keeping a hand-maintained object list

You can keep a configuration in source control that lists only the objects your application needs. Merge settings like these into `efcpt-config.json`, or into a copy of the sample configuration before the first run:

```json
{
   "code-generation": {
      "refresh-object-lists": false
   },
   "tables": [
      {
         "name": "[dbo].[Products]",
         "excludedColumns": [ "CreatedAt", "UpdatedAt" ]
      },
      {
         "name": "[dbo].[Categories]"
      }
   ],
   "views": [],
   "stored-procedures": [],
   "functions": []
}
```

This selects only the Products and Categories tables and omits the two audit columns from the generated entity for Products. `excludedColumns` contains database column names and can be set on individual table or view entries; see [column selection](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering#choose-database-objects) in the guide. New database objects will be included only when you add them to the configuration or re-enable `refresh-object-lists`.

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
