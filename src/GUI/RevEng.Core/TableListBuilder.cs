using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Model;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RevEng.Core
{
    public class TableListBuilder
    {
        private readonly string _connectionString;
        private readonly SchemaInfo[] _schemas;
        private readonly DatabaseType _databaseType;
        private readonly ServiceProvider _serviceProvider;

        public TableListBuilder(int databaseType, string connectionString, SchemaInfo[] schemas)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(@"invalid connection string", nameof(connectionString));
            }
            _connectionString = SqlServerHelper.SetConnectionString((DatabaseType)databaseType, connectionString);
            _schemas = schemas;
            _databaseType = (DatabaseType)databaseType;

            var options = new ReverseEngineerCommandOptions
            {
                DatabaseType = _databaseType,
            };

            _serviceProvider = ServiceProviderBuilder.Build(options);
        }

        public List<TableModel> GetTableModels()
        {
            var databaseTables = GetTableDefinitions();

            var buildResult = new List<TableModel>();

            foreach (var databaseTable in databaseTables)
            {
                var columns = new List<ColumnModel>();

                var primaryKeyColumnNames = databaseTable.PrimaryKey?.Columns.Select(c => c.Name).ToHashSet();
                var foreignKeyColumnNames = databaseTable.ForeignKeys?.SelectMany(c => c.Columns).Select(c => c.Name).ToHashSet();
                var indexColumns = databaseTable.Indexes?.SelectMany(c => c.Columns);

                foreach (var colum in databaseTable.Columns.Where(c => !string.IsNullOrWhiteSpace(c.Name)))
                {
                    columns.Add(new ColumnModel(colum.Name, primaryKeyColumnNames?.Contains(colum.Name) ?? false, foreignKeyColumnNames?.Contains(colum.Name) ?? false));
                }

                buildResult.Add(new TableModel(databaseTable.Name, databaseTable.Schema, _databaseType, databaseTable is DatabaseView ? ObjectType.View : ObjectType.Table, columns));
            }

            return buildResult;
        }

        private List<DatabaseTable> GetTableDefinitions()
        {
            var dbModelFactory = _serviceProvider.GetService<IDatabaseModelFactory>();

            var dbModelOptions = new DatabaseModelFactoryOptions(schemas: _schemas?.Select(s => s.Name));
            var dbModel = dbModelFactory.Create(_connectionString, dbModelOptions);

            return dbModel.Tables.ToList();
        }

        public List<TableModel> GetProcedures()
        {
            var result = new List<TableModel>();

            if (_databaseType != DatabaseType.SQLServer && _databaseType != DatabaseType.SQLServerDacpac)
            {
                return result;    
            }

            var procedureModelFactory = _serviceProvider.GetService<IProcedureModelFactory>();

            var procedureModelOptions = new ModuleModelFactoryOptions
            {
                FullModel = false,
                Modules = new List<string>(),
            };

            var procedureModel = procedureModelFactory.Create(_connectionString, procedureModelOptions);

            foreach (var procedure in procedureModel.Routines)
            {
                result.Add(new TableModel(procedure.Name, procedure.Schema, _databaseType, ObjectType.Procedure, null));
            }

            return result.OrderBy(c => c.DisplayName).ToList();
        }

        public List<TableModel> GetFunctions()
        {
            var result = new List<TableModel>();

            if (_databaseType != DatabaseType.SQLServer)
            {
                return result;
            }

            var functionModelFactory = _serviceProvider.GetService<IFunctionModelFactory>();

            var functionModelOptions = new ModuleModelFactoryOptions
            {
                FullModel = false,
                Modules = new List<string>(),
            };

            var functionModel = functionModelFactory.Create(_connectionString, functionModelOptions);

            foreach (var function in functionModel.Routines)
            {
                result.Add(new TableModel(function.Name, function.Schema, _databaseType, ObjectType.ScalarFunction, null));
            }

            return result.OrderBy(c => c.DisplayName).ToList();
        }
    }
}
