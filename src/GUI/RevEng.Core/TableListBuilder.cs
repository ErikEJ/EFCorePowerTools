using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using RevEng.Core.Procedures;
using RevEng.Core.Procedures.Model;
using ReverseEngineer20.ReverseEngineer;
using System;
using System.Collections.Generic;
using System.Linq;
using EFCorePowerTools.Shared.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
#if CORE50
#else
using Oracle.EntityFrameworkCore.Design.Internal;
#endif

namespace ReverseEngineer20
{
    public class TableListBuilder
    {
        private readonly string _connectionString;
        private readonly SchemaInfo[] _schemas;
        private readonly DatabaseType _databaseType;
        private readonly ServiceProvider serviceProvider;

        public TableListBuilder(int databaseType, string connectionString, SchemaInfo[] schemas)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(@"invalid connection string", nameof(connectionString));
            }
            _connectionString = connectionString;
            _schemas = schemas;
            _databaseType = (DatabaseType)databaseType;

            if (_databaseType == DatabaseType.SQLServer)
            {
                var builder = new SqlConnectionStringBuilder(_connectionString)
                {
                    CommandTimeout = 300
                };
                _connectionString = builder.ConnectionString;
            }

            serviceProvider = Build(_databaseType);
        }

        public List<TableModel> GetTableModels()
        {
            var databaseTables = GetTableDefinitions();

            var buildResult = new List<TableModel>();

            foreach (var databaseTable in databaseTables)
            {
                string name;
                if (_databaseType == DatabaseType.SQLServer)
                {
                    name = $"[{databaseTable.Schema}].[{databaseTable.Name}]";
                }
                else
                {
                    name = string.IsNullOrEmpty(databaseTable.Schema)
                        ? databaseTable.Name
                        : $"{databaseTable.Schema}.{databaseTable.Name}";
                }

                var columns = new List<ColumnModel>();

                var primaryKeyColumnNames = databaseTable.PrimaryKey?.Columns.Select(c => c.Name).ToHashSet();

                foreach (var colum in databaseTable.Columns)
                {
                    columns.Add(new ColumnModel(colum.Name, primaryKeyColumnNames?.Contains(colum.Name) ?? false));
                }

                buildResult.Add(new TableModel(name, databaseTable.Name, databaseTable.Schema, databaseTable is DatabaseView ? RevEng.Shared.ObjectType.View : RevEng.Shared.ObjectType.Table, columns));
            }

            return buildResult;
        }

        private List<Microsoft.EntityFrameworkCore.Scaffolding.Metadata.DatabaseTable> GetTableDefinitions()
        {
            var dbModelFactory = serviceProvider.GetService<IDatabaseModelFactory>();

            var dbModelOptions = new DatabaseModelFactoryOptions(schemas: _schemas?.Select(s => s.Name));
            var dbModel = dbModelFactory.Create(_connectionString, dbModelOptions);

            return dbModel.Tables.ToList();
        }

        public List<string> GetProcedures(int dbTypeInt)
        {
            var result = new List<string>();

            DatabaseType databaseType = (DatabaseType)dbTypeInt;

            if (databaseType != DatabaseType.SQLServer)
            {
                return result;    
            }

            var procedureModelFactory = serviceProvider.GetService<IProcedureModelFactory>();

            var procedureModelOptions = new ProcedureModelFactoryOptions
            {
                FullModel = false,
                Procedures = new List<string>(),
            };

            var procedureModel = procedureModelFactory.Create(_connectionString, procedureModelOptions);

            foreach (var procedure in procedureModel.Procedures)
            {
                result.Add($"[{procedure.Schema}].[{procedure.Name}]");
            }

            result.Sort();

            return result;
        }

        private static ServiceProvider Build(DatabaseType databaseType)
        {
            // Add base services for scaffolding
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddEntityFrameworkDesignTimeServices()
                .AddSingleton<IOperationReporter, OperationReporter>()
                .AddSingleton<IOperationReportHandler, OperationReportHandler>();

            // Add database provider services
            switch (databaseType)
            {
                case DatabaseType.SQLServer:
                    var provider = new SqlServerDesignTimeServices();
                    provider.ConfigureDesignTimeServices(serviceCollection);
                    serviceCollection.AddSqlServerStoredProcedureDesignTimeServices();
                    break;

                case DatabaseType.Npgsql:
                    var npgsqlProvider = new NpgsqlDesignTimeServices();
                    npgsqlProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;

                case DatabaseType.SQLite:
                    var sqliteProvider = new SqliteDesignTimeServices();
                    sqliteProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;

                case DatabaseType.Mysql:
                    var mysqlProvider = new MySqlDesignTimeServices();
                    mysqlProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;
#if CORE50
#else
                case DatabaseType.Oracle:
                    var oracleProvider = new OracleDesignTimeServices();
                    oracleProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;
#endif
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}
