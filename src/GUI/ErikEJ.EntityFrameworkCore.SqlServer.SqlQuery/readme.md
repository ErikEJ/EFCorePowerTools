# Materialize abritary classes and scalar values from EF Core using raw SQL

[![Nuget](https://img.shields.io/nuget/v/ErikEJ.EntityFrameworkCore.SqlServer.SqlQuery)](https://www.nuget.org/packages/ErikEJ.EntityFrameworkCore.SqlServer.SqlQuery/)

Provides the `SqlQueryAsync<T>` and `SqlQueryValueAsync<T>` methods to help you populate arbitrary classes or a list of primitive types from a raw SQL query.

# Sample usage


````csharp
var context = new ChinookContext();

var result = await context.SqlQueryValueAsync<int>("SELECT 1 AS Value");

var result2 = await context.SqlQueryValueAsync<string>("SELECT 'test' AS Value");
````

## Feedback

Please report any issues, questions and suggestions [here](https://github.com/ErikEJ/EFCorePowerTools/issues)
