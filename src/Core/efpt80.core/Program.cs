using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: CLSCompliant(true)]

namespace Modelling
{
    internal static class Program
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
