using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RevEng.Shared;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Abstractions;
#if CORE50
#else
using Oracle.EntityFrameworkCore.Design.Internal;
#endif

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

            _serviceProvider = TableListServiceProviderBuilder.Build(_databaseType);
        }

        public List<TableModel> GetTableModels()
        {
            var databaseTables = GetTableDefinitions();

            var buildResult = new List<TableModel>();

            foreach (var databaseTable in databaseTables)
            {
                var columns = new List<ColumnModel>();

                var primaryKeyColumnNames = databaseTable.PrimaryKey?.Columns.Select(c => c.Name).ToHashSet();

                foreach (var colum in databaseTable.Columns)
                {
                    columns.Add(new ColumnModel(colum.Name, primaryKeyColumnNames?.Contains(colum.Name) ?? false));
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

            var procedureModelOptions = new ProcedureModelFactoryOptions
            {
                FullModel = false,
                Procedures = new List<string>(),
            };

            var procedureModel = procedureModelFactory.Create(_connectionString, procedureModelOptions);

            foreach (var procedure in procedureModel.Procedures)
            {
                result.Add(new TableModel(procedure.Name, procedure.Schema, _databaseType, ObjectType.Procedure, null));
            }

            return result.OrderBy(c => c.DisplayName).ToList();
        }
    }
}
