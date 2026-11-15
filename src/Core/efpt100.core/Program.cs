using System;
using System.Linq;
using System.Text;

[assembly: CLSCompliant(true)]

namespace Modelling;

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
                var result = args.Contains("ddl") && args.Length >= 3 ? EfCoreModelBuilder.GenerateDatabaseCreateScript(args[1], args[2]) :
                             args.Contains("contextlist") && args.Length >= 3 ? EfCoreCompareBuilder.GenerateDbContextList(args[1], args[2]) :
                             args.Contains("schemacompare") && args.Length >= 5 ? EfCoreCompareBuilder.GenerateSchemaCompareResult(args[1], args[2], args[3], args[4]) :
                             EfCoreModelBuilder.GenerateDebugView(args[0], args[1]);

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