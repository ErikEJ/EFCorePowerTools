# Reverse engineer a SQL Server .dacpac with the EF Core tooling 

[![Nuget](https://img.shields.io/nuget/v/ErikEJ.EntityFrameworkCore.SqlServer.Dacpac)](https://www.nuget.org/packages/ErikEJ.EntityFrameworkCore.SqlServer.Dacpac/)

# Sample usage

`dotnet ef dbcontext scaffold ..\Db.dacpac ErikEJ.EntityFrameworkCore.SqlServer.Dacpac`

## Current conventions

Use the -t parameter(s) to specify individual tables in this format: [dbo].[MyTable]

## Known issues

If you would like to scaffold a seperate schema with Data Annotations, use the [workaround described here](https://github.com/ErikEJ/EFCorePowerTools/issues/435#issuecomment-666305362)

## Feedback

Please report any issues, questions and suggestions [here](https://github.com/ErikEJ/EFCorePowerTools/issues)
