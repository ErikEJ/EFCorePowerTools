using Npgsql;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace RevEng.Core.Routines.Procedures
{
    public class PostgresStoredProcedureModelFactory : PostgresRoutineModelFactory, IProcedureModelFactory
    {
        public PostgresStoredProcedureModelFactory()
        {
            RoutineType = "PROCEDURE";

            RoutineSql = $@"
select n.nspname as schema_name,
       p.proname as specific_name,
       p.proretset as returns_set,
       p.prokind as routine_type
from pg_proc p
left join pg_namespace n on p.pronamespace = n.oid
left join pg_language l on p.prolang = l.oid
left join pg_type t on t.oid = p.prorettype 
where n.nspname not in ('pg_catalog', 'information_schema')
      and (p.prokind = 'p' or p.prokind = 'f')
      and (l.lanname = 'sql' or l.lanname = 'plpgsql')
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

            using var dtResult = new DataTable();
            var list = new List<ModuleResultElement>();

            var sql = $@"
    with cte1 as (
        select
            lower(r.routine_type) as type,
            quote_ident(r.specific_schema) as schema,
            quote_ident(r.routine_name) as name,
            proc.oid as oid,
            r.specific_name,
            des.description as comment,
            proc.proisstrict as is_strict,
            proc.provolatile as volatility_option,
            proc.proretset as returns_set,
            (r.type_udt_schema || '.' || r.type_udt_name)::regtype::text as return_type,

            coalesce(
                array_agg(quote_ident(p.parameter_name::text) order by p.ordinal_position) filter(where p.parameter_mode = 'IN' or p.parameter_mode = 'INOUT'),
                '{{}}'::text[]
            ) as in_params,

            case when r.data_type = 'USER-DEFINED' then null
            else
                coalesce(
                    (array_agg(quote_ident(p.parameter_name::text) order by p.ordinal_position) 
                    filter(where p.parameter_mode = 'INOUT' or p.parameter_mode = 'OUT')),
                    '{{}}'::text[]
                )
            end as out_params,

            coalesce(
                array_agg(
                    case when p.data_type = 'bit' then 'varbit' else (p.udt_schema || '.' || p.udt_name)::regtype::text end
                    order by p.ordinal_position
                ) filter(where p.parameter_mode = 'IN' or p.parameter_mode = 'INOUT'),
                '{{}}'::text[]
            ) as in_param_types,

            case when r.data_type = 'USER-DEFINED' then null
            else
                coalesce(
                    (array_agg(case when p.data_type = 'bit' then 'varbit' else (p.udt_schema || '.' || p.udt_name)::regtype::text end 
                               order by p.ordinal_position) 
                    filter(where p.parameter_mode = 'INOUT' or p.parameter_mode = 'OUT')),
                    '{{}}'::text[]
                )
            end as out_param_types,

            coalesce(
                array_agg(p.parameter_default order by p.ordinal_position) filter(where p.parameter_mode = 'IN' or p.parameter_mode = 'INOUT'),
                '{{}}'::text[]
            ) as in_param_defaults,

            case when proc.provariadic <> 0 then true else false end as has_variadic,

            r.type_udt_schema,
            r.type_udt_name,
            r.data_type
        from
            information_schema.routines r
            join pg_catalog.pg_proc proc on r.specific_name = proc.proname || '_' || proc.oid
            left join pg_catalog.pg_description des on proc.oid = des.objoid
            left join information_schema.parameters p on r.specific_name = p.specific_name and r.specific_schema = p.specific_schema

        where
            r.specific_schema = any(
                select
                    nspname
                from
                    pg_catalog.pg_namespace
                where
                    nspname not like 'pg_%'
                    and nspname <> 'information_schema'
            )
            and proc.prokind in ('f', 'p')
            and not lower(r.external_language) = any(array['c', 'internal'])
            and coalesce(r.type_udt_name, '') <> 'trigger'
        group by
            r.routine_type, r.specific_schema, r.routine_name,
            proc.oid, r.specific_name, des.description,
            r.data_type, r.type_udt_schema, r.type_udt_name,
            proc.proisstrict, proc.procost, proc.prorows, proc.proparallel, proc.provolatile,
            proc.proretset, proc.provariadic

    ), cte2 as (

        select 
            cte1.schema, 
            cte1.specific_name,
            array_agg(quote_ident(col.column_name) order by col.ordinal_position) as out_params,
            array_agg(
                case when col.data_type = 'bit' then 'varbit' else (col.udt_schema || '.' || col.udt_name)::regtype::text end
                order by col.ordinal_position
            ) as out_param_types
        from 
            information_schema.columns col
            join cte1 on 
            cte1.data_type = 'USER-DEFINED' and col.table_schema = cte1.type_udt_schema and col.table_name = cte1.type_udt_name 
        group by
           cte1.schema, cte1.specific_name

    )
    select
        type,
        cte1.schema,
        cte1.name,
        is_strict,
        volatility_option,

        returns_set,
        coalesce(return_type, 'void') as return_type,

        coalesce(array_length(coalesce(cte1.out_params, cte2.out_params), 1), 1) as return_record_count,
        case 
            when array_length(coalesce(cte1.out_params, cte2.out_params), 1) is null 
            then array[cte1.name]::text[] 
            else coalesce(cte1.out_params, cte2.out_params) 
        end as return_record_names,

        case 
            when array_length(coalesce(cte1.out_param_types, cte2.out_param_types), 1) is null 
            then array[return_type]::text[] 
            else coalesce(cte1.out_param_types, cte2.out_param_types)
        end as return_record_types,

        coalesce(cte1.out_params, cte2.out_params) = '{{}}' as is_unnamed_record

    from cte1
    left join cte2 on cte1.schema = cte2.schema and cte1.specific_name = cte2.specific_name
	where cte1.schema = '{module.Schema}' and (cte1.name = '{module.Name}' or cte1.name = '""{module.Name}""')";

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            using var adapter = new NpgsqlDataAdapter
            {
                SelectCommand = new NpgsqlCommand(sql, connection),
            };
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

            adapter.Fill(dtResult);

            if (dtResult.Rows.Count != 0)
            {
                var row = dtResult.Rows[0];
                var hasSet = bool.Parse(row["returns_set"].ToString() ?? "false");
                if (hasSet)
                {
                    var names = (string[])row["return_record_names"];
                    var types = (string[])row["return_record_types"];
                    for (var i = 0; i < names.Length; i++)
                    {
                        var name = names[i];

                        if (name.StartsWith('"') && name.EndsWith('"'))
                        {
                            name = name.Substring(1, name.Length - 2);
                        }

                        list.Add(new ModuleResultElement
                        {
                            Name = name,
                            StoreType = types[i],
                            Ordinal = i,
                            Nullable = true,
                        });
                    }
                }
            }

            var result = new List<List<ModuleResultElement>>
            {
                list,
            };

            return result;
        }
    }
}
