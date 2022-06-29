using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Model;

namespace RevEng.Core
{
    public class TableListBuilder
    {
        private readonly string connectionString;
        private readonly SchemaInfo[] schemas;
        private readonly DatabaseType databaseType;
        private readonly ServiceProvider serviceProvider;

        public TableListBuilder(int databaseType, string connectionString, SchemaInfo[] schemas, bool mergeDacpacs)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), @"invalid connection string");
            }

            this.connectionString = SqlServerHelper.SetConnectionString((DatabaseType)databaseType, connectionString);
            this.schemas = schemas;
            this.databaseType = (DatabaseType)databaseType;

            var options = new ReverseEngineerCommandOptions
            {
                DatabaseType = this.databaseType,
                MergeDacpacs = mergeDacpacs,
            };

            serviceProvider = ServiceProviderBuilder.Build(options);
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
                columns.AddRange(from colum in databaseTable.Columns.Where(c => !string.IsNullOrWhiteSpace(c.Name))
                                 select new ColumnModel(
                                     colum.Name,
                                     primaryKeyColumnNames?.Contains(colum.Name) ?? false,
                                     foreignKeyColumnNames?.Contains(colum.Name) ?? false));
                buildResult.Add(new TableModel(databaseTable.Name, databaseTable.Schema, databaseType, databaseTable is DatabaseView ? ObjectType.View : ObjectType.Table, columns));
            }

            return buildResult;
        }

        public List<TableModel> GetProcedures()
        {
            var result = new List<TableModel>();

            if (databaseType != DatabaseType.SQLServer && databaseType != DatabaseType.SQLServerDacpac)
            {
                return result;
            }

            var procedureModelFactory = serviceProvider.GetService<IProcedureModelFactory>();

            var procedureModelOptions = new ModuleModelFactoryOptions
            {
                FullModel = false,
                Modules = new List<string>(),
            };

            var procedureModel = procedureModelFactory.Create(connectionString, procedureModelOptions);

            foreach (var procedure in procedureModel.Routines)
            {
                result.Add(new TableModel(procedure.Name, procedure.Schema, databaseType, ObjectType.Procedure, null));
            }

            return result.OrderBy(c => c.DisplayName).ToList();
        }

        public List<TableModel> GetFunctions()
        {
            var result = new List<TableModel>();

            if (databaseType != DatabaseType.SQLServer)
            {
                return result;
            }

            var functionModelFactory = serviceProvider.GetService<IFunctionModelFactory>();

            var functionModelOptions = new ModuleModelFactoryOptions
            {
                FullModel = false,
                Modules = new List<string>(),
            };

            var functionModel = functionModelFactory.Create(connectionString, functionModelOptions);

            foreach (var function in functionModel.Routines)
            {
                result.Add(new TableModel(function.Name, function.Schema, databaseType, ObjectType.ScalarFunction, null));
            }

            return result.OrderBy(c => c.DisplayName).ToList();
        }

        private List<DatabaseTable> GetTableDefinitions()
        {
            var dbModelFactory = serviceProvider.GetService<IDatabaseModelFactory>();

            var dbModelOptions = new DatabaseModelFactoryOptions(schemas: schemas?.Select(s => s.Name));
            var dbModel = dbModelFactory.Create(connectionString, dbModelOptions);

            return dbModel.Tables.ToList();
        }
    }
}
