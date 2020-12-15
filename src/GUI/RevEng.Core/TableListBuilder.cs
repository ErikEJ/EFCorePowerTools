using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using ReverseEngineer20.ReverseEngineer;
using System;
using System.Collections.Generic;
using System.Linq;
using EFCorePowerTools.Shared.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RevEng.Shared;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Abstractions;
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

            serviceProvider = TableListServiceProviderBuilder.Build(_databaseType);
        }

        public List<TableModel> GetTableModels()
        {
            var databaseTables = GetTableDefinitions();

            var buildResult = new List<TableModel>();

            foreach (var databaseTable in databaseTables)
            {
                string name;
                if (_databaseType == DatabaseType.SQLServer || _databaseType == DatabaseType.SQLServerDacpac)
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

                buildResult.Add(new TableModel(name, databaseTable.Name, databaseTable.Schema, databaseTable is DatabaseView ? ObjectType.View : ObjectType.Table, columns));
            }

            return buildResult;
        }

        private List<DatabaseTable> GetTableDefinitions()
        {
            var dbModelFactory = serviceProvider.GetService<IDatabaseModelFactory>();

            var dbModelOptions = new DatabaseModelFactoryOptions(schemas: _schemas?.Select(s => s.Name));
            var dbModel = dbModelFactory.Create(_connectionString, dbModelOptions);

            return dbModel.Tables.ToList();
        }

        public List<TableModel> GetProcedures(int dbTypeInt)
        {
            var result = new List<TableModel>();

            DatabaseType databaseType = (DatabaseType)dbTypeInt;

            if (databaseType != DatabaseType.SQLServer && databaseType != DatabaseType.SQLServerDacpac)
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
                result.Add(new TableModel($"[{procedure.Schema}].[{procedure.Name}]", procedure.Name, procedure.Schema, ObjectType.Procedure, null));
            }

            return result.OrderBy(c => c.DisplayName).ToList();
        }

    }
}
