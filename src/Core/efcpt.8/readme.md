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

### Configuring options

A configuration file `efcpt-config.json` is created in the output folder, and you can open this file in your editor to modify the default options. If your editor supports it (for example VS Code), it will provide syntax guidance for the file. For reference there is a fully populated sample file [here](https://github.com/ErikEJ/EFCorePowerTools/blob/master/samples/efcpt-config.json).

### Excluding PostgreSQL indexes

Use `excludedIndexes` on a table to omit specific PostgreSQL indexes from the scaffolded model:

```json
"tables": [
  {
    "name": "public.child",
    "excludedIndexes": ["uq_child_active_parent"]
  }
]
```

Use the exact schema-qualified table name from the generated configuration and exact index names (case-sensitive, no wildcards). Unknown names are ignored. In Visual Studio's `efpt.config.json`, the corresponding table settings are `Name` and `ExcludedIndexes`.

This can work around [EF Core #11298](https://github.com/dotnet/efcore/issues/11298): a partial unique index on foreign-key columns can incorrectly cause a one-to-one relationship to be scaffolded. Exclusions are applied before relationship inference. Foreign keys, columns, primary keys, and unique constraints are retained unless excluded by other settings; other unique indexes or keys can still imply a one-to-one relationship. Unlisted indexes are unchanged; filtered indexes are not automatically excluded.

The database index is not dropped or modified and continues to enforce uniqueness. Its mapping is omitted from the generated EF model, so review the consequences before using that model for migrations or database creation. Existing SQL Server index-exclusion behavior is unchanged.

### Updating to new configuration

After updating the `efcpt-config.json`, you will need to run the `efcpt` CLI command from above once again in order to update the generated code.

If you have updated the configuration file in a way that requires files to be deleted - by excluding objects for example - then you will need to set the `"soft-delete-obsolete-files"` option in the configuration file to `true` or delete the files manually.

### Excluding objects

The config file defaults to always contain all current database objects. 

If you don't want the lists of objects to be refreshed during each scaffolding operation, set the `"refresh-object-lists"` option in the configuration file to `false`.

You can exclude indvidual database objects with `"exclude": true` for the object.

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

The tool can generate a [Mermaid ER diagram](https://mermaid.js.org/syntax/entityRelationshipDiagram.html) during exectution, just set the `code-generation` option `generate-mermaid-diagram` to `true` and a `dbdiagram.md` file will be created in the output folder.

### Updating the tool

```bash
dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 8.*
```

[Release notes](https://github.com/ErikEJ/EFCorePowerTools/wiki/Release-notes) - notice the `+CLI` label.

### Getting the latest daily build

```bash
dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 8.*-*
```
