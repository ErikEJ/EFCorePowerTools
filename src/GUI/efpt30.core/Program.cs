using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelling
{
    public class Program
    {
        static int Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;

                if (args.Length > 0)
                {
                    var builder = new EfCoreModelBuilder();
                    var migrationsBuilder = new EfCoreMigrationsBuilder();
#if CORE50 
                    var compareBuilder = new EfCoreCompareBuilder();
#endif
                    List<Tuple<string, string>> result;

                    if (args.Contains("ddl") && args.Count() >= 3)
                    {
                        result = builder.GenerateDatabaseCreateScript(args[1], args[2]);
                    }
#if CORE50
                    else if (args.Contains("contextlist") && args.Count() >= 3)
                    {
                        result = compareBuilder.GenerateDbContextList(args[1], args[2]);
                    }
                    else if (args.Contains("schemacompare") && args.Count() >= 5)
                    {
                        result = compareBuilder.GenerateSchemaCompareResult(args[1], args[2], args[3], args[4]);
                    }
#endif
                    else if (args.Contains("migrationstatus") && args.Count() >= 3)
                    {
                        result = migrationsBuilder.GenerateMigrationStatusList(args[1], args[2]);
                    }
                    else if (args.Contains("migrate") && args.Count() >= 4)
                    {
                        result = migrationsBuilder.Migrate(args[1], args[2], args[3]);
                    }
                    else if (args.Contains("scriptmigration") && args.Count() >= 4)
                    {
                        result = migrationsBuilder.ScriptMigration(args[1], args[2], args[3]);
                    }
                    else if (args.Contains("addmigration") && args.Count() >= 7)
                    {
                        result = migrationsBuilder.AddMigration(args[1], args[2], args[3], args[4], args[5], args[6]);
                    }
                    else
                    {
                        result = builder.GenerateDebugView(args[0], args[1]);
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
        }
    }
}
