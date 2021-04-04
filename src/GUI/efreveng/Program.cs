using RevEng.Core;
using RevEng.Shared;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace efreveng
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;

                if (args.Length > 0)
                {
                    if ((args.Count() == 2 || args.Count() == 3) && int.TryParse(args[0], out int dbTypeInt))
                    {
                        SchemaInfo[] schemas = null;
                        if (args.Length == 3)
                        {
                            schemas = args[2].Split(',').Select(s => new SchemaInfo {Name = s}).ToArray();
                        }
                        var builder = new TableListBuilder(dbTypeInt, args[1], schemas);

                        var buildResult = builder.GetTableModels();

                        buildResult.AddRange(builder.GetProcedures());

                        buildResult.AddRange(builder.GetFunctions());

                        Console.Out.WriteLine("Result:");
                        Console.Out.WriteLine(buildResult.Write());

                        return 0;
                    }

                    if (!File.Exists(args[0]))
                    {
                        Console.Out.WriteLine("Error:");
                        Console.Out.WriteLine($"Could not open options file: {args[0]}");
                        return 1;
                    }

                    var options = ReverseEngineerOptionsExtensions.TryDeserialize(File.ReadAllText(args[0], System.Text.Encoding.UTF8));

                    if (options == null)
                    {
                        Console.Out.WriteLine("Error:");
                        Console.Out.WriteLine("Could not read options");
                        return 1;
                    }

                    var result = ReverseEngineerRunner.GenerateFiles(options);

                    Console.Out.WriteLine("Result:");
                    Console.Out.WriteLine(result.Write());
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
                Console.Out.WriteLine(ex.Demystify());
                return 1;
            }
        }
    }
}
