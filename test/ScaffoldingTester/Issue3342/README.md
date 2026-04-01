# SQL database project with MSBuild.Sdk.SqlProj

## Build the project

To build the project, run the following command:

```bash
dotnet build
```

This will get you a dacpac at 'bin/Debug/net10.0/Issue3343.dacpac'

## EFCPT from a dacpac

```bash
efcpt 'bin/Debug/net10.0/Issue3343.dacpac' mssql
```

You will see your Models are generated for TableOne and TableTwo


## Edit efcpt-config.json

Now please update efcpt-config.json
You need to set `"refresh-object-lists": false`

and then please remove the TableTwo from the tables so that only TableOne remains
Make it looks like this
```
"tables": [
  {
    "name": "[dbo].[TableOne]"
  }
],
````

The intention here is that we want to leave TableTwo in the database project, but we want to remove it from the models.

Save the efcpt-config.json file.

Now run this again:
```bash
efcpt 'bin/Debug/net10.0/Issue3343.dacpac' mssql
```

Notice that Issue3343Context.cs is updated and TableTwo is removed

However the issue/bug is that the Models/TableTwo.cs is not removed