using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using ErikEJ.EfCorePowerTools.HostedServices;
using ErikEJ.EfCorePowerTools.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevEng.Common;
using RevEng.Common.Efcpt;
using RevEng.Core;

namespace ErikEJ.EfCorePowerTools;

internal static class RegisterDependentServices
{
    public static IHostBuilder RegisterServices(this IHostBuilder builder, DisplayService displayService, IFileSystem fileSystem, ScaffoldOptions scaffoldOptions)
    {
        var databaseType = scaffoldOptions.Provider.ToDatabaseType(scaffoldOptions.IsDacpac);
        var reverseOptions = new ReverseEngineerCommandOptions
        {
            DatabaseType = databaseType,
            MergeDacpacs = scaffoldOptions.IsDacpac,
            ConnectionString = scaffoldOptions.ConnectionString.ApplyDatabaseType(databaseType),
        };
        scaffoldOptions.ConnectionString = reverseOptions.ConnectionString;
        var hostBuilder = builder.ConfigureServices(
            (context, serviceCollection) =>
            {
                serviceCollection.AddEfpt(reverseOptions, new List<string>(), new List<string>(), new List<string>())
                    .AddSingleton(fileSystem)
                    .AddSingleton(displayService)
                    .AddSingleton(scaffoldOptions)
                    .AddSingleton(reverseOptions)
                    .AddSingleton<TableListBuilder>()
                    .AddSingleton<PackageService>()
                    .AddSingleton(Array.Empty<SchemaInfo>())
                    .AddHostedService<ScaffoldHostedService>();
            });

        if (databaseType != DatabaseType.Undefined)
        {
            return hostBuilder;
        }

        displayService.Error($"Unknown provider '{scaffoldOptions.Provider}' - valid values are: mssql, postgres, sqlite, oracle, mysql, firebird");
        Environment.ExitCode = 1;

        return hostBuilder;
    }
}
