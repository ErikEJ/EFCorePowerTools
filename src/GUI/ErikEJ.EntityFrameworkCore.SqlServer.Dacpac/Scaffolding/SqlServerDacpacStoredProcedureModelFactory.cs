using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using dac = Microsoft.SqlServer.Dac.Model;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using ReverseEngineer20.DacpacConsolidate;
using System.Collections.Generic;
using System.Linq;
using SqlSharpener;

namespace ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding
{
    public class SqlServerDacpacStoredProcedureModelFactory : IProcedureModelFactory
    {
        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        public SqlServerDacpacStoredProcedureModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
        {
            _logger = logger;
        }

        public ProcedureModel Create(string connectionString, ProcedureModelFactoryOptions options)
        {
            return GetStoredProcedures(connectionString, options);
        }

        private ProcedureModel GetStoredProcedures(string dacpacPath, ProcedureModelFactoryOptions options)
        {
            var consolidator = new DacpacConsolidator();
            dacpacPath = consolidator.Consolidate(dacpacPath);

            var model = new dac.TSqlModel(dacpacPath);
            var result = new List<Procedure>();

            var metaBuilder = new MetaBuilder();

            metaBuilder.LoadModel(model);

            var metaProcedures = metaBuilder.Procedures;

            foreach (var metaProcedure in metaProcedures)
            {
                var parameters = new List<ProcedureParameter>();
                var resultParameters = new List<ProcedureResultElement>();

                int ordinal = 0;
                foreach (var column in metaProcedure.Selects.First().Columns)
                {
                    resultParameters.Add(new ProcedureResultElement
                    {
                        Name = column.Name,
                        Nullable = column.IsNullable,
                        StoreType = column.DataTypes[TypeFormat.SqlServerDbType],
                        Ordinal = ordinal++,
                    }); 
                }
                var procedure = new Procedure
                {
                    Name = metaProcedure.Name,
                    Schema = metaProcedure.Schema,
                    Parameters = new List<ProcedureParameter>
                    {
                        
                    },
                    ResultElements = resultParameters,
                };

                result.Add(procedure);
            }


            return new ProcedureModel
            {
                Procedures = result,
                Errors = new List<string>(),
            };
        }
    }
}