using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using ErikEJ.EFCorePowerTools.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevEng.Common;
using RevEng.Core;

namespace ErikEJ.EFCorePowerTools.HostedServices;

internal static class RegisterDependentServices
{
    public static IHostBuilder RegisterServices(this IHostBuilder builder, IFileSystem fileSystem, ScaffoldOptions scaffoldOptions)
    {
        var databaseType = scaffoldOptions.Provider.ToDatabaseType(scaffoldOptions.IsDacpac);
        var reverseOptions = new ReverseEngineerCommandOptions
        {
            DatabaseType = databaseType,
            ConnectionString = scaffoldOptions.ConnectionString.ApplyDatabaseType(databaseType),
        };
        scaffoldOptions.ConnectionString = reverseOptions.ConnectionString;
        var hostBuilder = builder.ConfigureServices(
            (context, serviceCollection) =>
            {
                serviceCollection.AddEfpt(reverseOptions, new List<string>(), new List<string>(), new List<string>())
                    .AddSingleton(fileSystem)
                    .AddSingleton(scaffoldOptions)
                    .AddSingleton(reverseOptions)
                    .AddSingleton<TableListBuilder>()
                    .AddSingleton(Array.Empty<SchemaInfo>())
                    .AddHostedService<ScaffoldHostedService>();
            });

        if (databaseType != DatabaseType.Undefined)
        {
            return hostBuilder;
        }

        DisplayService.Error($"Unknown provider '{scaffoldOptions.Provider}' - valid values are: mssql, sqlserver, postgres, postgresql, sqlite, oracle, mysql, firebird");
        Environment.ExitCode = 1;

        return hostBuilder;
    }
}
