# Materialize abritary classes from EF Core using raw SQL

[![Nuget](https://img.shields.io/nuget/v/ErikEJ.EntityFrameworkCore.SqlServer.SqlQuery)](https://www.nuget.org/packages/ErikEJ.EntityFrameworkCore.SqlServer.SqlQuery/)

Provides the `SqlQueryAsync<T>` method to help you populate arbitrary classes from a raw SQL query.

# Sample usage


````csharp
var context = new ChinookContext();

var result = await context.SqlQueryAsync<MyDto>("SELECT * FROM MyTable");
````

## Feedback

Please report any issues, questions and suggestions [here](https://github.com/ErikEJ/EFCorePowerTools/issues)
