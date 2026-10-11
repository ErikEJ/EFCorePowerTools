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

### Keep generated models in sync in CI

You can check in a build pipeline that the generated code agrees with the database schema. Then a pull request that changes the database, but not the generated code (or the opposite), fails the build. With a .dacpac, no SQL Server is required.

1. Pin the version of the tool with a local tool manifest, and commit the manifest file (`dotnet-tools.json`), so that developers and the pipeline use the same version:

   ```bash
   dotnet new tool-manifest
   dotnet tool install ErikEJ.EFCorePowerTools.Cli --version 10.*
   ```

   The manifest records the exact version that was installed. Developers and the pipeline then run `dotnet tool restore`, and use `dotnet efcpt` instead of `efcpt`.

2. In `efcpt-config.json`, set these options, so that the generated files are the same on every operating system, and the config file does not change during the check. Use the values that agree with your repository, for example with your `.editorconfig`. `file-charset` and `file-line-endings` need version 10.1.1529 or later.

   ```json
   "code-generation": {
      "file-charset": "utf-8",
      "file-line-endings": "crlf",
      "refresh-object-lists": false
   }
   ```

3. In the pipeline, build the .dacpac, generate the code, and fail the build if a generated file is new, changed or deleted. `efcpt` exits with a non-zero code when it fails, so a failed run also fails the build. Use `git status`, not `git diff`, for the check: `git diff` does not show new files, for example the entity class of a new table.

   For example, with GitHub Actions, when the generated code is in `src/DataAccess/Models`:

   ```yaml
   - uses: actions/checkout@v7

   - uses: actions/setup-dotnet@v6
     with:
       dotnet-version: '10.0.x'

   - run: dotnet tool restore

   - run: dotnet build src/Database/Database.csproj

   - run: dotnet efcpt ../Database/bin/Debug/net10.0/Database.dacpac -i efcpt-config.json
     working-directory: src/DataAccess

   - name: Check that the generated code is committed
     working-directory: src/DataAccess
     shell: bash
     run: |
       git status --porcelain -- Models
       test -z "$(git status --porcelain -- Models)"
   ```

   If you set `output-dbcontext-path`, also check that folder.

A .dacpac can have less type information for views and stored procedure results than a live database, see [SQL Server Database project (.dacpac) mapping](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering#sql-server-database-project-dacpac-mapping). If you need a live database, publish the .dacpac to a SQL Server container in the pipeline, and generate the code from the connection string.

### Updating the tool

```bash
dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 8.*
```

[Release notes](https://github.com/ErikEJ/EFCorePowerTools/wiki/Release-notes) - notice the `+CLI` label.

### Getting the latest daily build

```bash
dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 8.*-*
```
