using Npgsql;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using System;
using System.Collections.Generic;

namespace RevEng.Core.Procedures
{
    public class PostgresStoredProcedureModelFactory : PostgresRoutineModelFactory, IProcedureModelFactory
    {
        public PostgresStoredProcedureModelFactory()
        {
            RoutineType = "PROCEDURE";

            RoutineSql = $@"
select n.nspname as schema_name,
       p.proname as specific_name,
       CAST(0 as boolean) as is_scalar
from pg_proc p
left join pg_namespace n on p.pronamespace = n.oid
left join pg_language l on p.prolang = l.oid
left join pg_type t on t.oid = p.prorettype 
where n.nspname not in ('pg_catalog', 'information_schema')
      and p.prokind = 'p'
order by schema_name,
         specific_name;";
        }

        public RoutineModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            return GetRoutines(connectionString, options);
        }

        protected override List<List<ModuleResultElement>> GetResultElementLists(NpgsqlConnection connection, Routine module)
        {
            ArgumentNullException.ThrowIfNull(connection);

            ArgumentNullException.ThrowIfNull(module);

            return new List<List<ModuleResultElement>>();
        }
    }
}
