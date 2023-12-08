using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: CLSCompliant(true)]

namespace Modelling
{
    public static class Program
    {
        public static int Main(string[] args)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                Console.OutputEncoding = Encoding.UTF8;

                ArgumentNullException.ThrowIfNull(args);

                if (args.Length > 0)
                {
                    List<Tuple<string, string>> result;

                    if (args.Contains("ddl") && args.Length >= 3)
                    {
                        result = EfCoreModelBuilder.GenerateDatabaseCreateScript(args[1], args[2]);
                    }
#if EFCOMPARE
                    else if (args.Contains("contextlist") && args.Length >= 3)
                    {
                        result = EfCoreCompareBuilder.GenerateDbContextList(args[1], args[2]);
                    }
                    else if (args.Contains("schemacompare") && args.Length >= 5)
                    {
                        result = EfCoreCompareBuilder.GenerateSchemaCompareResult(args[1], args[2], args[3], args[4]);
                    }
#endif
                    else if (args.Contains("migrationstatus") && args.Length >= 3)
                    {
                        result = EfCoreMigrationsBuilder.GenerateMigrationStatusList(args[1], args[2]);
                    }
                    else if (args.Contains("migrate") && args.Length >= 4)
                    {
                        result = EfCoreMigrationsBuilder.Migrate(args[1], args[2], args[3]);
                    }
                    else if (args.Contains("scriptmigration") && args.Length >= 4)
                    {
                        result = EfCoreMigrationsBuilder.ScriptMigration(args[1], args[2], args[3]);
                    }
                    else if (args.Contains("addmigration") && args.Length >= 7)
                    {
                        result = EfCoreMigrationsBuilder.AddMigration(args[1], args[2], args[3], args[4], args[5], args[6]);
                    }
                    else
                    {
                        result = EfCoreModelBuilder.GenerateDebugView(args[0], args[1]);
                    }

                    foreach (var tuple in result)
                    {
                        Console.Out.WriteLine("DbContext:");
                        Console.Out.WriteLine(tuple.Item1);
                        Console.Out.WriteLine("DebugView:");
                        Console.Out.WriteLine(tuple.Item2);
                    }
                }
                else
                {
                    Console.Out.WriteLine("Error:");
                    Console.Out.WriteLine("Invalid command line");
                    return 1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Error:");
                Console.Out.WriteLine(ex);
                return 1;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
