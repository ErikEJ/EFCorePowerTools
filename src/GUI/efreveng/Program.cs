using EFCorePowerTools.Shared.Models;
using ReverseEngineer20;
using ReverseEngineer20.ReverseEngineer;
using System;
using System.Collections.Generic;
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
                    if (args[0].EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase))
                    { 
                        return BuildDacpacList(args[0]);
                    }

                    if ((args.Count() == 2 || args.Count() == 3) && int.TryParse(args[0], out int dbTypeInt))
                    {
                        SchemaInfo[] schemas = null;
                        if (args.Length == 3)
                        {
                            schemas = args[2].Split(',').Select(s => new SchemaInfo {Name = s}).ToArray();
                        }
                        var builder = new TableListBuilder(dbTypeInt, args[1], schemas);

                        var buildResult = builder.GetTableModels();

                        var procedures = builder.GetProcedures(dbTypeInt);
                        buildResult.AddRange(procedures.Select(p => new TableModel(p, null, null, RevEng.Shared.ObjectType.Procedure, null)).ToList());

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

                    var runner = new ReverseEngineerRunner();

                    var result = runner.GenerateFiles(options);

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
                Console.Out.WriteLine(ex);
                return 1;
            }
        }

        private static int BuildDacpacList(string dacpacPath)
        {
            if (!File.Exists(dacpacPath))
            {
                Console.Out.WriteLine("Error:");
                Console.Out.WriteLine($"Could not open .dacpac file: {dacpacPath}");
                return 1;
            }

            var builder = new DacpacTableListBuilder(dacpacPath);

            var values = builder.GetTableDefinitions();

            var result = new List<TableModel>();

            foreach (var value in values)
            {
                var columns = new List<ColumnModel>();

                foreach (var colum in value.Item4)
                {
                    columns.Add(new ColumnModel(colum, value.Item4.Contains(colum)));
                }

                result.Add(new TableModel(value.Item3, value.Item1, value.Item2, value.Item6 ? RevEng.Shared.ObjectType.Table : RevEng.Shared.ObjectType.View, columns));
            }
        
            Console.Out.WriteLine("Result:");
            Console.Out.WriteLine(result.Write());

            return 0;
        }
    }
}
